using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoodsStore.Models
{
    public class APIData
    {
        [Key]
        public int Id { get; set; }


        public string Value { get; set; }

        public string Value2 { get; set; }

    }
}
