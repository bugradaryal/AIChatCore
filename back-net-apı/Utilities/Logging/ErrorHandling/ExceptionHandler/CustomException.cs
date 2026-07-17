using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Logging.ErrorHandling.ExceptionHandler
{
    public class CustomException : Exception
    {
        public int ErrorCode { get; }        // HTTP status code (400, 404, 500 vb.)
        public int ExceptionLevel { get; }   // LogLevel karşılığı (1:Info ... 4:Fatal)

        public CustomException(string message, int exceptionLevel = (int) ELog.Fatal, int errorCode = 500)
            : base(message) 
        {
            ErrorCode = errorCode;
            ExceptionLevel = exceptionLevel;
        }
    }
}
