using System.ComponentModel.DataAnnotations;

namespace GoodsStore.Models
{
    public class Category
    {
        public int Id { get; set; }

        /// <summary>
        /// Название категории
        /// </summary>
        [Display(Name = "Категория товара")]
        public string NameCategory { get; set; }
    }
}