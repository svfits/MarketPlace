using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoodsStore.ViewsModel
{
    /// <summary>
    /// Логин модель
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
