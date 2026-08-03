using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace BlackCatPomodoro
{
    /// <summary>
    /// 数据持久化服务 -- XML 文件读写，零外部依赖
    /// </summary>
    public class DataService
    {
        private static readonly string DataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "BlackCatPomodoro");

        private static readonly string DataFile = Path.Combine(DataFolder, "tasks.xml");

        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(TaskListData));

        /// <summary>
        /// 从文件加载所有待办，文件不存在时返回含默认示例的列表
        /// </summary>
        public List<PomodoroTask> Load()
        {
            try
            {
                if (!File.Exists(DataFile))
                    return CreateDefault();

                using (var fs = new FileStream(DataFile, FileMode.Open, FileAccess.Read))
                {
                    var data = (TaskListData)_serializer.Deserialize(fs);
                    return data.Tasks ?? new List<PomodoroTask>();
                }
            }
            catch
            {
                return CreateDefault();
            }
        }

        /// <summary>
        /// 保存待办列表到文件，返回是否成功
        /// </summary>
        public bool Save(List<PomodoroTask> tasks)
        {
            try
            {
                if (!Directory.Exists(DataFolder))
                    Directory.CreateDirectory(DataFolder);

                var data = new TaskListData { Tasks = tasks };

                // 先写临时文件，成功后再替换，防止写坏原文件
                string tmpFile = DataFile + ".tmp";
                using (var fs = new FileStream(tmpFile, FileMode.Create, FileAccess.Write))
                {
                    _serializer.Serialize(fs, data);
                }
                File.Copy(tmpFile, DataFile, overwrite: true);
                File.Delete(tmpFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private List<PomodoroTask> CreateDefault()
        {
            return new List<PomodoroTask>
            {
                new PomodoroTask
                {
                    Name = "[示例] 学习编程",
                    FocusMinutes = 25,
                    RestMinutes = 5,
                    RepeatCount = 4,
                    Notes = "这是一个示例待办，可以删除或修改"
                }
            };
        }
    }
}
