using System;
using System.Net;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;
using Serilog;

namespace PdfMerger.Infrastructure
{
    public class PollyPolicyBuilder
    {   
        public static IAsyncPolicy<HttpResponseMessage> BuildRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, timeSpan, context) =>
                {
                    Log.Logger.Error("Retry StatusCode:{StatusCode}, Url: {RequestUri}", exception?.Result?.StatusCode, exception?.Result?.RequestMessage?.RequestUri);
                });
        }
    }

}
