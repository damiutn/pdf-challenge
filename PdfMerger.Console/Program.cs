using System;
using System.Collections.Generic;
using System.Linq;
using PdfMerger.Domain;

namespace PdfMerger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
                throw new BusinessException("At least one argument is needed");

            List<Uri> uris = new List<Uri>();
            List<string> badArguments = new List<string>();

            
            foreach (string arg in args)
            {
                if(Uri.IsWellFormedUriString(arg, UriKind.Absolute) )
                    uris.Add(new Uri(arg));
                else
                {
                    badArguments.Add(arg);
                }
            }

            if (badArguments.Count > 0)
                throw new BusinessException($"Invalid arguments: {string.Join(",", badArguments)}");

        }
    }
}
