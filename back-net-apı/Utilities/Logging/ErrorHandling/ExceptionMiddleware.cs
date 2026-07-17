using DTO;
using Entities;
using Logging.ErrorHnadling.LoggerManagement;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Utilities.Logging.ErrorHandling.ExceptionHandler;

namespace Utilities.Logging.ErrorHandling
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILoggerServices loggerServices)
        {
            try
            {
                await _next(context);   // pipeline'ın geri kalanını çalıştır (Controller, Business, hepsi burada tetiklenir)
            }
            catch (Exception ex)
            {
                int statusCode = 500;
                int logLevel = 4;

                if (ex is CustomException customEx)
                {
                    statusCode = customEx.ErrorCode;
                    logLevel = customEx.ExceptionLevel;
                }

                await loggerServices.Logger(new LogDTO
                {
                    LogMessage = ex.Message,
                    LogLevel = logLevel,
                    Target = context.Request.Path
                }, ex);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var errorResponse = new DTOErrorResponse
                {
                    message = ex.Message,
                    errorCode = statusCode.ToString()
                };

                var json = JsonConvert.SerializeObject(errorResponse);
                await context.Response.WriteAsync(json);
            }
        }
    }
}