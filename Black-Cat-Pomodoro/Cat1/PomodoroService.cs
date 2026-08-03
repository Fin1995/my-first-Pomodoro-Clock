using System;
using System.Windows.Forms;

namespace BlackCatPomodoro
{
    /// <summary>
    /// 番茄钟核心状态机
    /// </summary>
    public enum PomodoroPhase
    {
        Idle,       // 空闲
        Focusing,   // 专注中
        Resting     // 休息中
    }

    /// <summary>
    /// 番茄钟计时服务 -- 状态机 + System.Windows.Forms.Timer 每秒触发
    /// </summary>
    public class PomodoroService : IDisposable
    {
        private readonly Timer _timer;
        private int _totalSecondsThisPhase;
        private int _roundIndex; // 0-based

        // ---- 公开状态 ----
        public PomodoroPhase Phase { get; private set; } = PomodoroPhase.Idle;
        public bool IsPaused { get; private set; }
        public bool IsRunning => Phase != PomodoroPhase.Idle;
        public int RemainingSeconds { get; private set; }
        public int CurrentRound => _roundIndex + 1;  // 1-based for display
        public int TotalRounds { get; private set; }
        public PomodoroTask CurrentTask { get; private set; }

        // ---- 事件 ----
        /// <summary>每秒触发: (剩余秒数, 本阶段总秒数)</summary>
        public event Action<int, int> Tick;
        /// <summary>阶段切换触发</summary>
        public event Action PhaseChanged;
        /// <summary>全部循环完成触发</summary>
        public event Action CycleCompleted;

        public PomodoroService()
        {
            _timer = new Timer { Interval = 1000 };
            _timer.Tick += OnTick;
        }

        /// <summary>
        /// 开始新的番茄钟会话
        /// </summary>
        public void Start(PomodoroTask task)
        {
            CurrentTask = task.Clone();
            TotalRounds = CurrentTask.RepeatCount;
            _roundIndex = 0;
            IsPaused = false;

            SwitchToFocus();
            _timer.Start();
            PhaseChanged?.Invoke();
        }

        /// <summary>
        /// 暂停 / 继续
        /// </summary>
        public void TogglePause()
        {
            if (Phase == PomodoroPhase.Idle) return;

            IsPaused = !IsPaused;
            PhaseChanged?.Invoke();
        }

        /// <summary>
        /// 跳过当前阶段
        /// </summary>
        public void Skip()
        {
            if (Phase == PomodoroPhase.Idle) return;

            // 设为 1 秒，让下一次 timer tick 自然触发切换，避免直接调 OnTick 产生 -1
            RemainingSeconds = 1;
            IsPaused = false;
        }

        /// <summary>
        /// 停止并回到空闲状态
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
            Phase = PomodoroPhase.Idle;
            IsPaused = false;
            RemainingSeconds = 0;
            PhaseChanged?.Invoke();
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (IsPaused) return;
            if (Phase == PomodoroPhase.Idle) return;

            RemainingSeconds--;
            if (RemainingSeconds >= 0)
                Tick?.Invoke(RemainingSeconds, _totalSecondsThisPhase);

            if (RemainingSeconds <= 0)
            {
                if (Phase == PomodoroPhase.Focusing)
                {
                    // 专注结束 -> 进入休息
                    SwitchToRest();
                    PhaseChanged?.Invoke();
                }
                else
                {
                    // 休息结束
                    _roundIndex++;
                    if (_roundIndex < TotalRounds)
                    {
                        // 还有下一轮
                        SwitchToFocus();
                        PhaseChanged?.Invoke();
                    }
                    else
                    {
                        // 全部完成
                        _timer.Stop();
                        Phase = PomodoroPhase.Idle;
                        IsPaused = false;
                        RemainingSeconds = 0;
                        CycleCompleted?.Invoke();
                        PhaseChanged?.Invoke();
                    }
                }
            }
        }

        private void SwitchToFocus()
        {
            Phase = PomodoroPhase.Focusing;
            _totalSecondsThisPhase = CurrentTask.FocusMinutes * 60;
            RemainingSeconds = _totalSecondsThisPhase;
        }

        private void SwitchToRest()
        {
            Phase = PomodoroPhase.Resting;
            _totalSecondsThisPhase = CurrentTask.RestMinutes * 60;

            // 如果休息时间为 0，直接跳过
            if (_totalSecondsThisPhase <= 0)
            {
                _roundIndex++;
                if (_roundIndex < TotalRounds)
                {
                    SwitchToFocus();
                }
                else
                {
                    _timer.Stop();
                    Phase = PomodoroPhase.Idle;
                    RemainingSeconds = 0;
                    CycleCompleted?.Invoke();
                }
            }
            else
            {
                RemainingSeconds = _totalSecondsThisPhase;
            }
        }

        public void Dispose()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }
    }
}
