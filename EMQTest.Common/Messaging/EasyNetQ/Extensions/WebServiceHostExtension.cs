using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMQTest.Common.Messaging.EasyNetQ.Extensions
{
    public static partial class WebServiceHostExtension
    {
        public static BusBuilder UseEasyNetQ(this IWebHostBuilder source, string connectionString)
        {
            return new BusBuilder(source, connectionString);
        }
    }
}
