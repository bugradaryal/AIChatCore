using DTO;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Logging.Serilog.SerilogManager
{
    public class SerilogLogger : ISerilogServices
    {
        public void Info(LogDTO logDTO)
        {
            Log.Information("{@Log}", logDTO);
        }

        public void Warn(LogDTO logDTO)
        {
            Log.Warning("{@Log}", logDTO);
        }

        public void Error(LogDTO logDTO, Exception ex = null)
        {
            Log.Error(ex, "{@Log}", logDTO);
        }

        public void Fatal(LogDTO logDTO, Exception ex = null)
        {
            Log.Fatal(ex, "{@Log}", logDTO);
        }
    }
}