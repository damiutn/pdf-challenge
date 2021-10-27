using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PdfMerger.Test
{
    public class ProgramTest
    {
        //Note that it is not necesary to put a meaningful name due the DisplayName of Fact 

        /// <summary>
        /// This is the first test that forces me create the code
        /// </summary>
        [Fact(DisplayName = "When call "+nameof(Program.Main)+" with correct parameter then there is not exception")]
        public void Test1()
        {
            //Arrage
            List<string> urlWithPdfList = new List<string>();
            
            //Act
            Action act=()=> Program.Main(urlWithPdfList.ToArray());
            //Assert
            act.Should().NotThrow("Exception is not expected");

        }
    }
}
