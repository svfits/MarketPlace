using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GoodsStore.ViewsModel;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GoodsStore.Models
{
    public class DataContextApp : IdentityDbContext<User, IdentityRole, string>, IDataContextApp
    {
        public DataContextApp(DbContextOptions<DataContextApp> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Basket> Baskets { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<RegisterViewModel> RegisterViewModel { get; set; }

        public DbSet<APIData> APIDatas { get; set; }

        /// <summary>
        /// Задачи
        /// </summary>
        public DbSet<TasksService> TasksServices { get; set; }       
    }
}
