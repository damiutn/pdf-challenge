using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PdfMerger.Domain;
using PdfMerger.Domain.Exceptions;

namespace PdfMerger.Application
{
    public interface IPdfMergeService
    {
        Task<string> MergePdfsAsync(string[] urls);
    }

    public class PdfMergeService : IPdfMergeService
    {
        private readonly IContentExtractor _contentExtractor;
        private readonly IPdf _pdf;

        public PdfMergeService(IContentExtractor contentExtractor, IPdf pdf)
        {
            _contentExtractor = contentExtractor;
            _pdf = pdf;
        }

        public async Task<string> MergePdfsAsync(string[] urls)
        {
            ValidateUrlFormat(urls);

            var contents = _contentExtractor.GetContents(urls);
            var pdfMerged = _pdf.MergePdfs(contents);

            string pathToSavePdf = ".\\pdfs\\";
            //Create the directory does not exist
            Directory.CreateDirectory(pathToSavePdf);

            string file = Path.Combine(pathToSavePdf, $"pdfMerged_{DateTime.Now:yyyyMMddHHmmss}.pdf");
            await File.WriteAllBytesAsync(file, pdfMerged);

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
