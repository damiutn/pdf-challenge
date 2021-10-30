using System;
using System.Net.Http;
using System.Threading.Tasks;
using PdfMerger.Domain.Exceptions;

namespace PdfMerger.Infrastructure
{
    public interface IExternalContentRepository
    {
        Task<byte[]> GetBinaryContentFromUlrAsync(string url);
    }

    public class ExternalContentRepository : IExternalContentRepository
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