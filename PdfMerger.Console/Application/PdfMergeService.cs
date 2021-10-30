using System;
using System.Collections.Generic;
using System.IO;
using PdfMerger.Domain;
using PdfMerger.Domain.Exception;

namespace PdfMerger.Application
{
    public class PdfMergeService
    {
        private readonly ContentExtractor _contentExtractor;
        private readonly Pdf _pdf;

        public PdfMergeService()
        {
            _contentExtractor = new ContentExtractor();
            _pdf = new Pdf();
        }

        public string MergePdfs(string[] urls)
        {
            ValidateUrlFormat(urls);

            var contents = _contentExtractor.GetContents(urls);
            var pdfMerged = _pdf.MergePdfs(contents);

            string pathToSavePdf = ".\\pdfs\\";
            //Create the directory does not exist
            Directory.CreateDirectory(pathToSavePdf);

            string file = Path.Combine(pathToSavePdf, $"pdfMerged_{DateTime.Now:yyyyMMddHHmmss}.pdf");
            File.WriteAllBytes(file, pdfMerged);

            return file;
        }

        private void ValidateUrlFormat(string[] urls)
        {
            if (urls == null || urls.Length == 0)
                throw new BusinessException("At least one argument is needed");

            List<string> badArguments = new List<string>();


            foreach (string arg in urls)
            {
                if (!Uri.IsWellFormedUriString(arg, UriKind.Absolute))
                    badArguments.Add(arg);
            }

            if (badArguments.Count > 0)
                throw new BusinessException($"Invalid arguments: {string.Join(",", badArguments)}");
        }
    }
}
