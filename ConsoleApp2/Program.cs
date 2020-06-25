using PdfSharp;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] fileContents;

            using (MemoryStream ms = new MemoryStream())
            {
                //var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(str, PdfSharp.PageSize.A4);
                var file = File.ReadAllText(@"D:\Projects\MarketPlace\ConsoleApp2\bin\Debug\NuGet Gallery _ HtmlRenderer.PdfSharp 1.5.1-beta1.html");

                PdfDocument pdf = PdfGenerator.GeneratePdf("<p><h1>Hello World</h1>This is html rendered text</p>", PageSize.A4);
                pdf.Save(ms);
                fileContents = ms.ToArray();
            }

            File.WriteAllBytes(@"ssss.pdf", fileContents);
        }
    }
}
