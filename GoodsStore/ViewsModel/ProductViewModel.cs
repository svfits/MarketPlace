using GoodsStore.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoodsStore.ViewsModel
{
    public class ProductViewModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(1000)]
        public string NameProduct { get; set; }

        /// <summary>
        /// Фотографии товара
        /// </summary>
        public IFormFileCollection Images { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        [DataType(DataType.Currency)]
        public int Cost { get; set; }

        /// <summary>
        /// Категория товара
        /// </summary>
        public  Category Category { get; set; }
    }
}
