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
            var data = LoadJson().AsQueryable();

            var mockSet = new Mock<DbSet<Basket>>();
                        
            mockSet.As<IQueryable<Basket>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Basket>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Basket>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Basket>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<IDataContextApp>();
            mockContext.Setup(c => c.Baskets).Returns(mockSet.Object);          

            var service = new ExcelController(mockContext.Object);
            var excelByte = service.Download();

            Assert.Fail();
        }

        [TestMethod()]
        public void DownloadTest()
        {
            Assert.Fail();
        }

        public List<Basket> LoadJson()
        {
            using StreamReader r = new StreamReader(@"D:\Projects\MarketPlace\GoodsStoreTests\DataContextTEST.json");
            string json = r.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Basket>>(json);
        }
    }
}