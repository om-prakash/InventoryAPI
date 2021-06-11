using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Inventory.API.Models
{
    public class Log
    {
        public string LoggerType { get; set; }
        public string ApplicationName { get; set; }
        public string ExceptionType { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string AdditionalInfo { get; set; }
        public string HostName { get; set; }
        public string UserName { get; set; }
    }
}
