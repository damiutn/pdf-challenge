using System;

namespace PdfMerger.Domain
{
    public class BusinessException:Exception
    {
        public BusinessException(string message):base(message)
        {
        }
    }
}