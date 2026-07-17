using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Enums;

namespace Entities
{
    public class Log
    {
        public int id { get; set; }
        public string LogMessage { get; set; } = null!;  
        public int LogLevel { get; set; } = (int)ELog.Info;         
        public string? Target { get; set; }          
        public string? ExceptionDetail { get; set; }      
        public int? session_id { get; set; }
        public DateTime log_date { get; set; }
    }
}
