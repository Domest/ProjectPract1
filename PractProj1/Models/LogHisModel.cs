using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PractProj1.Models
{
    public class LogHisModel
    {
        public LogHisModel(string name, string logtype, DateTime logtime)
        {
            Name = name;
            LogType = logtype;
            LogTime = logtime;
        }
        
        public string Name { get; set; }
        public string LogType { get; set; }
        public DateTime LogTime { get; set; }
    }
}
