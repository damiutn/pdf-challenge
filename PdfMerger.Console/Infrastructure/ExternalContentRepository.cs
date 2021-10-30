using System;
using System.Net.Http;
using System.Threading.Tasks;
using PdfMerger.Domain.Exception;

namespace PdfMerger.Infrastructure
{
    public class ExternalContentRepository
    {
        public async Task<byte[]> GetBinaryContentFromUlrAsync(string url)
        {
            Console.WriteLine("Processing " + url);
            using HttpClient client = new HttpClient();
            HttpResponseMessage msg = await client.GetAsync(url);

            if (!msg.IsSuccessStatusCode)
            {
                throw new BusinessException($"URL: {url}. Response code: {msg.IsSuccessStatusCode}");
            }
            var contentStream = await msg.Content.ReadAsByteArrayAsync();
            return contentStream;
        }
    }
}