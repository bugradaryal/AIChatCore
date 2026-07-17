using DTO;
using System;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ILoggerServices
    {
        Task Logger(LogDTO logDto, Exception? exception = null);
    }
}
