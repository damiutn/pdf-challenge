﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PdfMerger.Domain;
using PdfMerger.Domain.Exceptions;
using PdfMerger.Shared;

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
        private readonly IOptions<PdfMergerOptions> _config;

        public PdfMergeService(IContentExtractor contentExtractor, IPdf pdf,IOptions<PdfMergerOptions> config)
        {
            _contentExtractor = contentExtractor;
            _pdf = pdf;
            _config = config;
        }

        public async Task<string> MergePdfsAsync(string[] urls)
        {
            ValidateInput(urls);

            var contents = _contentExtractor.GetContents(urls);
            var pdfMerged = _pdf.MergePdfs(contents);

            string pathToSavePdf = _config.Value.PathToSaveMergedPdf;
            //Create the directory does not exist
            Directory.CreateDirectory(pathToSavePdf);

            string file = Path.Combine(pathToSavePdf, $"pdfMerged_{DateTime.Now:yyyyMMddHHmmss}.pdf");
            await File.WriteAllBytesAsync(file, pdfMerged);

            return file;
        }

        private void ValidateInput(string[] urls)
        {
            if (urls == null || urls.Length < 2)
                throw new BusinessException("At least two arguments are required");

            var badArguments = new List<string>();


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
