using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfMerger.Domain
{
    public interface IPdf
    {
        byte[] MergePdfs(List<byte[]> pdfContents);
    }

    public class Pdf : IPdf
    {
        public byte[] MergePdfs(List<byte[]> pdfContents)
        {
            using MemoryStream stream = new MemoryStream();
            Document doc = null;
            PdfCopy pdf = null;
            PdfReader reader = null;
            try
            {
                doc = new Document();
                pdf = new PdfCopy(doc, stream) { CloseStream = false };
                doc.Open();

                foreach (var content in pdfContents)
                {
                    reader = new PdfReader(content);
                    for (int i = 0; i < reader.NumberOfPages; i++)
                    {
                        var page = pdf.GetImportedPage(reader, i + 1);
                        pdf.AddPage(page);
                    }

                    pdf.FreeReader(reader);
                    reader.Close();
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);

            }
            finally
            {
                reader?.Close();
                doc?.Close();
                pdf?.Close();
            }

            return stream.ToArray();
        }
    }

}
