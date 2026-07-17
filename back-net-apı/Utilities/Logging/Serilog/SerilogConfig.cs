using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Logging.Serilog
{
    public class SerilogConfig
    {
        public static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                // ELASTIC KISMI: buraya ileride .WriteTo.Elasticsearch(...) eklenecek
                .CreateLogger();
        }
    }
}
