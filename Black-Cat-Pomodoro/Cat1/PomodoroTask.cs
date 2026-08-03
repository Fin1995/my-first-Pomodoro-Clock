using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BlackCatPomodoro
{
    /// <summary>
    /// 单个待办事项的数据模型
    /// </summary>
    public class PomodoroTask
    {
        [XmlAttribute]
        public string Id { get; set; } = string.Empty;

        [XmlElement]
        public string Name { get; set; } = string.Empty;

        [XmlElement]
        public int FocusMinutes { get; set; } = 25;

        [XmlElement]
        public int RestMinutes { get; set; } = 5;

        [XmlElement]
        public int RepeatCount { get; set; } = 1;

        [XmlElement]
        public string Notes { get; set; } = string.Empty;

        public PomodoroTask()
        {
            Id = Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        public PomodoroTask Clone()
        {
            return new PomodoroTask
            {
                Id = this.Id,
                Name = this.Name,
                FocusMinutes = this.FocusMinutes,
                RestMinutes = this.RestMinutes,
                RepeatCount = this.RepeatCount,
                Notes = this.Notes
            };
        }
    }

    /// <summary>
    /// 待办列表容器，用于 XML 序列化
    /// </summary>
    [XmlRoot("PomodoroTasks")]
    public class TaskListData
    {
        [XmlElement("Task")]
        public List<PomodoroTask> Tasks { get; set; } = new List<PomodoroTask>();
    }
}
