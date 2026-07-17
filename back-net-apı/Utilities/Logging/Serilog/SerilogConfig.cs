using Elastic.Serilog.Sinks;
using Microsoft.Extensions.Configuration;
using Serilog;

public class SerilogConfig
{
    public static void ConfigureLogging(IConfiguration configuration)
    {
        var elasticUrl = configuration["SerilogCORS:ElasticUrl"];
        var dataStream = configuration["SerilogCORS:DataStream"];

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.Elasticsearch(new[] { new Uri(elasticUrl!) }, opts =>
            {
                opts.DataStream =
                    new Elastic.Ingest.Elasticsearch.DataStreams.DataStreamName(dataStream!);
            })
            .CreateLogger();
    }
}