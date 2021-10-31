using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using FluentAssertions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace PdfMerger.IntegrationTest
{
    public class PdfMergerIntegrationTest: IDisposable, IClassFixture<ResourceEmulator>
    {
        private readonly ResourceEmulator _resourceEmulator;
        private readonly string _resourcesLoremIpsumPdf;

        public PdfMergerIntegrationTest(ResourceEmulator resourceEmulator)
        {
            _resourceEmulator = resourceEmulator;
            _resourcesLoremIpsumPdf = ".\\Resources\\lorem-ipsum.pdf";
        }


        [Fact(DisplayName = "When app is called with local pdf then it should respond ok")]
        public void Test01()
        {
            //arrage
            //emulating a site that returns pdf
            _resourceEmulator.ServiceMock.Given(Request.Create().UsingGet()).RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                //.WithDelay(10)
                .WithHeader("content-type", MediaTypeNames.Application.Pdf)
                .WithBodyFromFile(_resourcesLoremIpsumPdf));
            
            var urls = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                urls.Add($"{_resourceEmulator.ServiceMock.Urls[0]}/pdf_{i:00}.pdf");

            }

            //act

            Func<Task> func = async () => await Program.Main(urls.ToArray());
            //assert
            func.Should().NotBeNull();
        }

        public void Dispose()
        {
          
            _resourceEmulator.Dispose();
        }
    }
}
