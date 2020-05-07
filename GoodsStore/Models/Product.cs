using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoodsStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Наименование товара")]
        public string NameProduct { get; set; }

        /// <summary>
        /// Фотографии товара
        /// </summary>
        [Display(Name = "Изображение")]
        public IList<Images> Images { get; set;}

        /// <summary>
        /// Описание
        /// </summary>
        [Display(Name = "Описание")]
        public string Description { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        [Display(Name = "Стоимость")]
        public int Cost { get; set; }

        /// <summary>
        /// Категория товара
        /// </summary>
        public Category Category { get; set; }
    }
}
