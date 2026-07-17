using DataAccess.Abstract;
using DTO;
using Entities;
using System;
using System.Threading.Tasks;
using Utilities.Logging.Serilog.SerilogManager;

namespace Logging.ErrorHnadling.LoggerManagement
{
    public class LoggerManager : ILoggerServices
    {
        private readonly ISerilogServices _logger;
        private readonly ILogRepository _logRepository;

        public LoggerManager(ISerilogServices logger, ILogRepository logRepository)
        {
            _logger = logger;
            _logRepository = logRepository;
        }

        public async Task Logger(LogDTO logDto, Exception? exception = null)
        {
            try
            {
                var entry = new Log
                {
                    LogMessage = logDto.LogMessage,
                    LogLevel = logDto.LogLevel,
                    Target = logDto.Target,
                    ExceptionDetail = exception?.ToString(),
                    session_id = logDto.session_id
                };

                if (logDto.LogLevel < 3)
                    _logger.Info(logDto);
                else if (logDto.LogLevel == 3)
                    _logger.Warn(logDto);
                else
                    _logger.Error(logDto, exception);

                await _logRepository.AddLogAsync(entry);

                // ELASTIC KISMI: buraya ileride Elastic'e de ayrı bir yazma çağrısı eklenebilir
            }
            catch (Exception ex)
            {
                // Loglama sırasında bile bir hata olursa (örn. DB bağlantısı yok),
                // en azından dosyaya/konsola Fatal seviyede düşürelim ki iz kaybolmasın
                _logger.Fatal(logDto, ex);
            }
        }
    }
}