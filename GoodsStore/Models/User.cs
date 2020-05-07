using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoodsStore.Models
{
    public class User : IdentityUser
    {
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public override string Email { get; set; }

        [Display(Name = "Имя")]
        public override string UserName { get; set; }        
    }    
}