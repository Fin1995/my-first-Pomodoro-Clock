using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace BlackCatPomodoro
{
    public enum SoundMode { SystemBeep, CustomFile, Silent }

    public class AudioDevice
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public override string ToString() => Name;
    }

    /// <summary>
    /// 音频服务 -- NAudio 可选依赖，不可用时自动降级为系统提示音
    /// </summary>
    public class AudioService : IDisposable
    {
        private bool _naudioOk;
        private IDisposable _player;
        private IDisposable _reader;

        public AudioService()
        {
            _naudioOk = TryProbeNAudio();
        }

        private static bool TryProbeNAudio()
        {
            try { ProbeNAudio(); return true; }
            catch (Exception) { return false; }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ProbeNAudio()
        {
            var _ = typeof(NAudio.Wave.WaveOutEvent);
        }

        public static List<AudioDevice> GetDevices()
        {
            var list = new List<AudioDevice>();
            try { GetDevicesNAudio(list); } catch (Exception) { }
            return list;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void GetDevicesNAudio(List<AudioDevice> list)
        {
            for (int i = 0; i < NAudio.Wave.WaveOut.DeviceCount; i++)
            {
                var caps = NAudio.Wave.WaveOut.GetCapabilities(i);
                list.Add(new AudioDevice { Index = i, Name = caps.ProductName });
            }
        }

        public void Play(string filePath, int deviceIndex = -1)
        {
            Stop();
            if (!_naudioOk || !File.Exists(filePath))
            { PlaySystemBeep(); return; }
            try { PlayNAudio(filePath, deviceIndex); }
            catch (Exception) { PlaySystemBeep(); }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void PlayNAudio(string path, int devIdx)
        {
            var reader = new NAudio.Wave.AudioFileReader(path);
            NAudio.Wave.IWavePlayer player;
            if (devIdx >= 0 && devIdx < NAudio.Wave.WaveOut.DeviceCount)
                player = new NAudio.Wave.WaveOutEvent { DeviceNumber = devIdx };
            else
                player = new NAudio.Wave.WaveOutEvent();
            player.Init(reader);
            player.Play();
            _reader = reader; _player = player;
        }

        public static void PlaySystemBeep()
        {
            // Win11 上 Beep 可能无声，优先用 Exclamation，再用 Asterisk 兜底
            try { System.Media.SystemSounds.Exclamation.Play(); return; } catch { }
            try { System.Media.SystemSounds.Asterisk.Play(); return; } catch { }
            try { System.Media.SystemSounds.Beep.Play(); return; } catch { }
            try { Console.Beep(800, 300); } catch { }
        }

        public void Stop()
        {
            try
            {
                var stopMethod = _player?.GetType().GetMethod("Stop");
                stopMethod?.Invoke(_player, null);
            }
            catch { }
            _reader?.Dispose(); _reader = null;
            _player?.Dispose(); _player = null;
        }

        public void Dispose() { Stop(); }
    }
}
