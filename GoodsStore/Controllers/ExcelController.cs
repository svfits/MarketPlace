using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoodsStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace GoodsStore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ExcelController : Controller
    {
        private readonly IDataContextApp _context;

        public ExcelController(IDataContextApp context)
        {
            _context = context;
        }

        public IActionResult Download()
        {
            byte[] fileContents;
            //var productInBasket = _context.Baskets
            //    .Include(a => a.User)
            //    .Include(q => q.Product)
            //    .Include(o => o.Product.Category)
            //    .ToList();

            string path = Path.Combine(@"D:\Projects\MarketPlace\GoodsStoreTests\TestingFile", "DataContextTEST.json");
            using StreamReader r = new StreamReader(path);
            string json = r.ReadToEnd();
            var productInBasket = JsonConvert.DeserializeObject<List<Basket>>(json);

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
    }
}
