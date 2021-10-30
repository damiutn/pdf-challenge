using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using PdfMerger.Application;
using PdfMerger.Domain;
using Xunit;

namespace PdfMerger.Test.Application
{
    public class PdfMergeServiceTest
    {
        [Fact(DisplayName="When I call "+nameof(PdfMergeService.MergePdfsAsync)+" with at least 2 url It should merge those and return a the merged pdf  ")]
        public async Task Test01()
        {
            //arrange
            var pdfMock = new Mock<IPdf>();
            var contentExtractorMock = new Mock<IContentExtractor>();
            var pdfMergeService = new PdfMergeService(contentExtractorMock.Object, pdfMock.Object);
            var param = new[] { "http://url/pdf.pdf", "http://url/pdf.pdf" };
            var pdfsContent = new List<byte[]>();
            contentExtractorMock.Setup(f => f.GetContents(param)).Returns(pdfsContent);
            var mergedPdf = new byte[1222];
            pdfMock.Setup(f => f.MergePdfs(pdfsContent)).Returns(mergedPdf);
            //act

            string pdfMergedFileName = await pdfMergeService.MergePdfsAsync(param);
            //assert
            pdfMergedFileName.Should().NotBeNullOrEmpty();
        }
    }
}
