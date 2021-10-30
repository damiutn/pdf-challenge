﻿using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PdfMerger.Domain;
using PdfMerger.Infrastructure;
using Xunit;

namespace PdfMerger.Test.Domain
{
    public class ContentExtractorTest
    {
        [Fact(DisplayName = "When I call " + nameof(ContentExtractor.GetContents) + " It should return a list of contents")]
        public void Test01()
        {
            //arrange
            var externalContentRepositoryMock = new Mock<IExternalContentRepository>();
            var contentExtractorMock = new Mock<ILogger<ContentExtractor>>();
            var contentExtractor = new ContentExtractor(contentExtractorMock.Object, externalContentRepositoryMock.Object);
            var param = new[] { "http://url/pdf.pdf", "http://url/pdf.pdf" };
            byte[] value = new byte[2]{1,2};
            externalContentRepositoryMock.Setup(f => f.GetBinaryContentFromUlrAsync(It.IsAny<string>()))
                .ReturnsAsync(value);
            //act

            var result = contentExtractor.GetContents(param);
            //assert
            result.Should().HaveCount(param.Length);

            externalContentRepositoryMock.Verify(f => f.GetBinaryContentFromUlrAsync(param[0]));
            externalContentRepositoryMock.Verify(f => f.GetBinaryContentFromUlrAsync(param[1]));
        }
    }
}