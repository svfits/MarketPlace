using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodsStore.Models;
using GoodsStore.ViewsModel;
using Microsoft.AspNetCore.Identity;

namespace GoodsStore.Models
{
    public class DataContextApp : IdentityDbContext<User, IdentityRole, string>
    {
        public DataContextApp(DbContextOptions<DataContextApp> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Basket> Baskets { get; set; }        

        public DbSet<GoodsStore.Models.Product> Product { get; set; }        

        public DbSet<GoodsStore.Models.Category> Category { get; set; }        

        public DbSet<GoodsStore.ViewsModel.RegisterViewModel> RegisterViewModel { get; set; }

        public DbSet<APIData> APIDatas { get; set; }
    }
}
