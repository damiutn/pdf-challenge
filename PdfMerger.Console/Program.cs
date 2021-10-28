using PdfMerger.Domain;

namespace PdfMerger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
                throw new BusinessException("At least one argument is needed");
        }
    }
}
