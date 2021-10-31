using System;
using WireMock.Server;
using WireMock.Settings;

namespace PdfMerger.IntegrationTest
{
    public class ResourceEmulator : IDisposable
    {

        public ResourceEmulator()
        {
            ServiceMock = WireMockServer.Start(new WireMockServerSettings
            {
                
                StartAdminInterface = true,
            });
        }

        public WireMockServer ServiceMock { get; set; }

        public void Dispose()
        {
            ServiceMock.Stop();
            ServiceMock.Dispose();
        }
    }
}
