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

namespace GoodsStore.Controllers.Tests
{
    [TestClass()]
    public class ExcelControllerTests
    {
        [TestMethod()]
        public void ExcelControllerTest()
        {
            //var data = new List<>
            //{
            //    new Blog { Name = "BBB" },
            //    new Blog { Name = "ZZZ" },
            //    new Blog { Name = "AAA" },
            //}.AsQueryable();

            var data = LoadJson().AsQueryable();

            var mockSet = new Mock<DbSet<Basket>>();
            mockSet.As<IQueryable<Basket>>()
                .Setup(m => m.GetEnumerator())
                .Returns(new TestDbAsyncEnumerator<Basket>(data.GetEnumerator()));

            //mockSet.As<IQueryable<Blog>>()
            //    .Setup(m => m.Provider)
            //    .Returns(new TestDbAsyncQueryProvider<Blog>(data.Provider));

            //mockSet.As<IQueryable<Blog>>().Setup(m => m.Expression).Returns(data.Expression);
            //mockSet.As<IQueryable<Blog>>().Setup(m => m.ElementType).Returns(data.ElementType);
            //mockSet.As<IQueryable<Blog>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            //var mockContext = new Mock<BloggingContext>();
            //mockContext.Setup(c => c.Blogs).Returns(mockSet.Object);

            //var service = new BlogService(mockContext.Object);
            //var blogs = await service.GetAllBlogsAsync();


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