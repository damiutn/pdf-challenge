using System;
using PdfMerger.Application;

namespace PdfMerger
{
    public class Program
    {
        public static void Main(string[] args)
        {
           

            if (args == null || args.Length < 2)
                throw new ArgumentException("At least 2 parameters are expected");

            var pdfMergeService = new PdfMergeService();
            Console.WriteLine("Starting process");
            var pdfMerdedCreated=pdfMergeService.MergePdfs(args);
            
            Console.WriteLine($"PDF file created {pdfMerdedCreated}");
            Console.WriteLine("Finishing process");
        }

     
    }
}
