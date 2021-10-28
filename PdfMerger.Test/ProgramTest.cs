using System;
using System.Collections.Generic;
using Bogus;
using FluentAssertions;
using PdfMerger.Domain;
using Xunit;

namespace PdfMerger.Test
{
    public class ProgramTest
    {
        //Note that methods do not need to be given a meaningful name due to the DisplayName of Fact

        /// <summary>
        /// This is the first test that forces me create the code
        /// </summary>
        [Fact(DisplayName = "When call "+nameof(Program.Main)+" with correct parameter then there is not exception")]
        public void Test1()
        {
            //Arrage
            List<string> urlWithPdfList = new List<string>();
           
            urlWithPdfList.Add(new Faker().Internet.UrlWithPath("http", null, "AnyThing"));//todo:fix extension in the next iteration

            //Act
            Action act =()=> Program.Main(urlWithPdfList.ToArray());
            //Assert
            act.Should().NotThrow("Exception is not expected");

        }


        [Fact(DisplayName = "When call " + nameof(Program.Main) + " with an empty argument, a business exception should be thrown")]
        public void Test2()
        {
            //Arrage
            List<string> urlWithPdfList = new List<string>();

            //Act
            Action act = () => Program.Main(urlWithPdfList.ToArray());
            //Assert
            act.Should().Throw<BusinessException>("Exception is expected");

        }
    }
}