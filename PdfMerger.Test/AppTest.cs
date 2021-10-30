using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PdfMerger.Application;
using PdfMerger.Domain.Exceptions;
using Xunit;

namespace PdfMerger.Test
{
    public class AppTest
    {
        private readonly App _app;

        public AppTest()
        {
            var loggerMock = new Mock<ILogger<App>>();
            var pdfMergeServiceMock = new Mock<IPdfMergeService>();
            _app = new App(loggerMock.Object, pdfMergeServiceMock.Object);
        }
        //Note that methods do not need to be given a meaningful name due to the DisplayName of Fact

        /// <summary>
        /// This is the first test that forces me create the code
        /// </summary>
        [Theory(DisplayName = "When call "+nameof(App.RunAsync)+" with correct parameter then there is not exception")]
        [InlineData(new object[]{new[] {"http://oneurl/file.pdf", "http://oneurl/file.anyExtension"}})]
        [InlineData(new object[] { new[] { "https://oneurl/file.anyExtension", "http://oneurl/blabla/file.anyExtension"}})]
        public async Task Test1(string[] url)
        {
            //Arrage
            //Act
            Func<Task> act = async ()=>await _app.RunAsync(url);
            //Assert
            await act.Should().NotThrowAsync("Exception is not expected");

        }


        [Fact(DisplayName = "When call " + nameof(App.RunAsync) + " with an empty argument, a business exception should be thrown")]
        public void Test2()
        {
            //Arrage
            var urlWithPdfList = new List<string>();

            //Act
            Func<Task> act = () => _app.RunAsync(urlWithPdfList.ToArray());
            //Assert
            act.Should().ThrowAsync<ArgumentException>("Exception is expected");

        }

        [Theory(DisplayName = "When call " + nameof(App.RunAsync) + " with url with invalid pdf extension, a business exception should be thrown")]
        [InlineData("jjjj//invalidpdfurl")]
        [InlineData("anything")]
        [InlineData("http://jdudtggd   /anything")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Test3(string badUris)
        {
            //Arrage
            var urlWithPdfList = new List<string>{ badUris };

            //Act
            Func<Task> act = () => _app.RunAsync(urlWithPdfList.ToArray());
            //Assert
            act.Should().ThrowAsync<BusinessException>("Exception is not expected");

        }
    }
}
