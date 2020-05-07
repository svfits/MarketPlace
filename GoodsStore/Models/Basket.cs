using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoodsStore.Models
{
    public class Basket
    {
        [Key]
        public int Id { get; set; }

        public Product Product { get; set; }

        public User User { get; set; }
    }
}
