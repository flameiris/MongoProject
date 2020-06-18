using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kafka.Logs.Extensions
{
    public static class KafkaLogsMiddlewareExtensions
    {
        public static async Task<IApplicationBuilder> UseKafaLogs(this IApplicationBuilder builder)
        {

            return builder;
        }
    }
}
