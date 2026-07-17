using DTO;
using System;
using System.Threading.Tasks;

namespace Logging.ErrorHnadling.LoggerManagement
{
    public interface ILoggerServices
    {
        Task Logger(LogDTO logDto, Exception? exception = null);
    }
}
