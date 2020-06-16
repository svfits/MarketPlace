using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoodsStore.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using GoodsStore.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GoodsStore.Controllers.Tests
{
    [TestClass()]
    public class ExcelControllerTests
    {
        [TestMethod()]
        public void ExcelControllerTest()
        {
            var data = LoadTestDataExcel().AsQueryable();

            var mockSet = new Mock<DbSet<Basket>>();

            mockSet.As<IQueryable<Basket>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Basket>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Basket>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Basket>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IDataContextApp>();
            mockContext.Setup(c => c.Baskets).Returns(mockSet.Object);

            var service = new ExcelController(mockContext.Object);
            var excelByte = (Microsoft.AspNetCore.Mvc.FileContentResult)service.Download();

            File.WriteAllBytes(@"D:\Projects\MarketPlace\GoodsStoreTests\TestingFile\FileTarget.xlsx", excelByte.FileContents);

            Assert.AreEqual(excelByte.FileContents.Count(), LoadTestFileSample().ToList().Count, "Файлы не совпадают что то произошло");
        }

        public List<Basket> LoadTestDataExcel()
        {
            string path = Path.Combine(GetPathTestFile(), "DataContextTEST.json");
            using StreamReader r = new StreamReader(path);
            string json = r.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Basket>>(json);
        }

        public byte[] LoadTestFileSample()
        {
            string path = Path.Combine(GetPathTestFile(), "Заказанные товары.xlsx");
            var file = File.ReadAllBytes(path);
            return file;
        }

        private static string GetPathTestFile()
        {
            return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\", "TestingFile"));
        }
    }
}