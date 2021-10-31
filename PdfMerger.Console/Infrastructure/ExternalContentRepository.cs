using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PdfMerger.Domain.Exceptions;

namespace PdfMerger.Infrastructure
{
    public interface IExternalContentRepository
    {
        Task<byte[]> GetBinaryContentFromUlrAsync(string url);
    }

    public class ExternalContentRepository : IExternalContentRepository
    {
        private readonly ILogger<ExternalContentRepository> _logger;
       // private readonly IHttpClientFactory _httpClientFactory;

        public ExternalContentRepository(ILogger<ExternalContentRepository> logger)//, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
          //  _httpClientFactory = httpClientFactory;
        }
        public async Task<byte[]> GetBinaryContentFromUlrAsync(string url)
        {
            _logger.LogInformation($"Processing {url}");
            var sw = new Stopwatch();
            sw.Start();
            //using var client = _httpClientFactory.CreateClient("pdfClient");
            using var client = new HttpClient();
            HttpResponseMessage msg = await client.GetAsync(url);

            if (!msg.IsSuccessStatusCode)
            {

                throw new BusinessException($"URL: {url}. Response code: {msg.IsSuccessStatusCode}");
            }
            var contentStream = await msg.Content.ReadAsByteArrayAsync();
            sw.Stop();
            _logger.LogInformation($"Processed {url} in {sw.ElapsedMilliseconds} ms");
            return contentStream;
        }
    }
}