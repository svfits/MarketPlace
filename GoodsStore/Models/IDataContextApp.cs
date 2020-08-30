using GoodsStore.ViewsModel;
using Microsoft.EntityFrameworkCore;

namespace GoodsStore.Models
{
    public interface IDataContextApp
    {
        DbSet<APIData> APIDatas { get; set; }
        DbSet<Basket> Baskets { get; set; }
        DbSet<Category> Category { get; set; }
        DbSet<Product> Product { get; set; }
        DbSet<RegisterViewModel> RegisterViewModel { get; set; }

        DbSet<TasksService> TasksServices { get; set; }
    }
}