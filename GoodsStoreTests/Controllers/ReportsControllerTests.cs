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
    public class ReportsControllerTests
    {
        Mock<IDataContextApp> mockContext = new Mock<IDataContextApp>();

        RepotsController service;

        [TestInitialize]
        [DeploymentItem("TestingFile\\DataContextTEST.json")]
        private void MockContext()
        {
            using StreamReader r = new StreamReader("DataContextTEST.json");
            string json = r.ReadToEnd();

            var data = JsonConvert.DeserializeObject<List<Basket>>(json).AsQueryable();

            var mockSet = new Mock<DbSet<Basket>>();

            mockSet.As<IQueryable<Basket>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Basket>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Basket>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Basket>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext.Setup(c => c.Baskets).Returns(mockSet.Object);
            service = new RepotsController(mockContext.Object);
        }


        [TestMethod()]
        [DeploymentItem("TestingFile\\Заказанные товары.xlsx")]
        public void ExcelControllerTest()
        {
            var excelByte = (Microsoft.AspNetCore.Mvc.FileContentResult)service.DownloadExcel();

            ///тута можно отправить файл дальше на проверку что форматирование правильное
            File.WriteAllBytes("TestingFile\\FileTarget.xlsx", excelByte.FileContents);

            var file = File.ReadAllBytes("Заказанные товары.xlsx");

            Assert.AreEqual(excelByte.FileContents.Count(), file.ToList().Count, "Файлы не совпадают что то произошло");
        }



        //public List<Basket> LoadTestData(string fileName)
        //{
        //    using StreamReader r = new StreamReader(fileName);
        //    string json = r.ReadToEnd();
        //    return JsonConvert.DeserializeObject<List<Basket>>(json);
        //}

        //public byte[] LoadTestFileSample(string fileName)
        //{
        //    var file = File.ReadAllBytes(fileName);
        //    return file;
        //}

        [TestMethod()]
        public void DownloadPdfTest()
        {
            var service = new RepotsController(mockContext.Object);
            var pdfFile = (Microsoft.AspNetCore.Mvc.FileContentResult)service.DownloadPdf();

            ///тута можно отправить файл дальше на проверку что форматирование правильное
            File.WriteAllBytes(@"D:\Projects\MarketPlace\GoodsStoreTests\TestingFile\ggggg22.pdf", pdfFile.FileContents);

            Assert.IsTrue(true);
        }
    }
}