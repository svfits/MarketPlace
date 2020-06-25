using System.IO;
using System.Linq;
using GoodsStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.Generic;
using PdfSharp.Fonts;
using System;
using Spire.Xls;
using System.Diagnostics;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using PdfSharp;

namespace GoodsStore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RepotsController : Controller
    {
        private readonly IDataContextApp _context;

        public RepotsController(IDataContextApp context)
        {
            _context = context;
        }

        public IActionResult DownloadExcel()
        {
            byte[] fileContents;
            var productInBasket = GetProductBasket();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                worksheet.Cells[1, 1].Value = "Наименование товара";
                worksheet.Cells[1, 2].Value = "Артикул";
                worksheet.Cells[1, 3].Value = "Категория";
                worksheet.Cells[1, 4].Value = "Стоимость";
                worksheet.Cells[1, 5].Value = "Email заказчика";

                //Раскрасим шапку
                for (int i = 1; i <= 5; i++)
                {
                    worksheet.Cells[1, i].Style.Font.Size = 12;
                    worksheet.Cells[1, i].Style.Font.Bold = true;
                    worksheet.Cells[1, i].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                }

                for (int i = 2; i < (productInBasket.Count + 2); i++)
                {
                    worksheet.Cells[i, 1].Value = productInBasket[i - 2].Product.NameProduct;
                    worksheet.Cells[i, 2].Value = productInBasket[i - 2].Product.Id;
                    worksheet.Cells[i, 3].Value = productInBasket[i - 2].Product.Category.NameCategory;
                    worksheet.Cells[i, 4].Value = productInBasket[i - 2].Product.Cost;
                    worksheet.Cells[i, 5].Value = productInBasket[i - 2].User.Email;
                }

                int count = (productInBasket.Count + 2);
                worksheet.Cells[count, 5].Value = "Итого товаров:" + productInBasket.Count;
                worksheet.Cells[count, 5].Style.Font.Size = 16;
                worksheet.Cells[count, 5].Style.Font.Bold = true;
                worksheet.Cells[count, 5].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                int countSum = (productInBasket.Count + 3);
                worksheet.Cells[countSum, 5].Value = "На сумму:" + productInBasket.Sum(a => a.Product.Cost);
                worksheet.Cells[countSum, 5].Style.Font.Size = 16;
                worksheet.Cells[countSum, 5].Style.Font.Bold = true;
                worksheet.Cells[countSum, 5].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                fileContents = package.GetAsByteArray();
            }

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "Заказанные товары.xlsx"
            );
        }

        private List<Basket> GetProductBasket()
        {
            return _context.Baskets
                .Include(a => a.User)
                .Include(q => q.Product)
                .Include(o => o.Product.Category)
                .ToList();
        }

        public IActionResult DownloadPdf()
        {
            byte[] fileContents;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Сделано по заказу";

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

            // Draw the text
            //gfx.DrawString("Привет МИР!!! ККККККККККККККККкккккккккккккк", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            //gfx.DrawString("Привет МИР!!! ККККККККККККККККкккккккккккккк", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            //gfx.DrawString("Привет МИР!!! ККККККККККККККККкккккккккккккк", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            //gfx.DrawString("Привет МИР!!! ККККККККККККККККкккккккккккккк", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            //gfx.DrawString("Привет МИР!!! ККККККККККККККККкккккккккккккк", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            //gfx.DrawString("Привет МИР!!! ККККККККККККККККкккккккккккккк", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            //gfx.DrawString("Привет МИР!!! ККККККККККККККККкккккккккккккк", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            //gfx.DrawString("Привет МИР!!! ККККККККККККККККкккккккккккккк", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            //gfx.DrawString("Привет МИР!!! ККККККККККККККККкккккккккккккк", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            //gfx.DrawString("Привет МИР!!! ККККККККККККККККкккккккккккккк", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            gfx.DrawString("Привет МИР!!! ", font, XBrushes.Black, 22, 45);

            //gfx.DrawString("Привет МИР!!! ККККККККККККККККкккккккккккккк", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.BaseLineRight);
            //gfx.DrawString("Привет МИР!!! ККККККККККККККККкккккккккккккк", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.BaseLineLeft);


            MemoryStream ms = new MemoryStream();
            document.Save(ms);
            fileContents = ms.ToArray();
            ms.Dispose();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }

            return File(
                fileContents: fileContents,
                contentType: "application/pdf",
                fileDownloadName: "Заказанные товары.pdf"
            );
        }

        public IActionResult DowloadPdfFromExcel()
        {
            byte[] fileContents;

            Workbook workbook = new Workbook();

            var excelByte = (FileContentResult)DownloadExcel();

            var str = @"D:\Projects\MarketPlace\GoodsStoreTests\TestingFile\NuGet Gallery _ HtmlRenderer.PdfSharp 1.5.0.6.mhtml";

            //MemoryStream msExcel = new MemoryStream(excelByte.FileContents);

            //MemoryStream msPDF = new MemoryStream();

            //workbook.LoadTemplateFromFile(str);

            //workbook.SaveToStream(msPDF, FileFormat.PDF);

            //fileContents = msPDF.ToArray();
            //msExcel.Dispose();
            //msPDF.Dispose();

            using (MemoryStream ms = new MemoryStream())
            {
                //var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(str, PdfSharp.PageSize.A4);
                PdfDocument pdf = PdfGenerator.GeneratePdf("<p><h1>Hello World</h1>This is html rendered text</p>", PageSize.A4);
                pdf.Save(ms);
                fileContents = ms.ToArray();
            }

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }

            return File(
                fileContents: fileContents,
                contentType: "application/pdf",
                fileDownloadName: "Заказанные товары.pdf"
            );
        }
    }
}
