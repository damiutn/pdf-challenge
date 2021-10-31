using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Logging;
using PdfMerger.Domain.Exceptions;

namespace PdfMerger.Domain
{
    public interface IPdf
    {
        byte[] MergePdfFiles(string[] pdfFiles);
    }

    public class Pdf : IPdf
    {
        private readonly ILogger _logger;

        public Pdf(ILogger<Pdf> logger)
        {
            _logger = logger;
        }
        public byte[] MergePdfFiles(string[] pdfFiles)
        {
            ValidateInput(pdfFiles);
            using MemoryStream stream = new ();
            Document doc = null;
            PdfCopy pdf = null;
            PdfReader reader;
            try
            {
                doc = new Document();
                pdf = new PdfCopy(doc, stream) { CloseStream = false };
                doc.Open();

                foreach (var tempFile in pdfFiles)
                {
                    reader = new PdfReader(tempFile);
                    for (int i = 0; i < reader.NumberOfPages; i++)
                    {
                        var page = pdf.GetImportedPage(reader, i + 1);
                        pdf.AddPage(page);
                    }

                    pdf.FreeReader(reader);
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;

            }
            finally
            {
               
                pdf?.Close(); //It is not possible to use using
                doc?.Close(); //It is not possible to use using
               
            }

            return stream.ToArray();
        }

        private void ValidateInput(string[] pdfs)
        {
            if (pdfs.Length < 2)
                throw new BusinessException("At least 2 items are required");

        }
    }

}
