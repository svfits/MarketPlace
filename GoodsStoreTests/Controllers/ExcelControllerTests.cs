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
        [DeploymentItem("TestingFile\\Заказанные товары.xlsx")]
        [DeploymentItem("TestingFile\\DataContextTEST.json")]
        public void ExcelControllerTest()
        {
            var data = LoadTestData("DataContextTEST.json").AsQueryable();

            var mockSet = new Mock<DbSet<Basket>>();

            mockSet.As<IQueryable<Basket>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Basket>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Basket>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Basket>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IDataContextApp>();
            mockContext.Setup(c => c.Baskets).Returns(mockSet.Object);

            var service = new ExcelController(mockContext.Object);
            var excelByte = (Microsoft.AspNetCore.Mvc.FileContentResult)service.Download();

            ///тута можно отправить файл дальше на проверку что форматирование правильное
            File.WriteAllBytes("TestingFile\\FileTarget.xlsx", excelByte.FileContents);

            Assert.AreEqual(excelByte.FileContents.Count(), LoadTestFileSample("Заказанные товары.xlsx").ToList().Count, "Файлы не совпадают что то произошло");
        }

        public List<Basket> LoadTestData(string fileName)
        {
            using StreamReader r = new StreamReader(fileName);
            string json = r.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Basket>>(json);
        }

        public byte[] LoadTestFileSample(string fileName)
        {
            var file = File.ReadAllBytes(fileName);
            return file;
        }
    }
}