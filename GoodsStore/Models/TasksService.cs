using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoodsStore.Models
{
    /// <summary>
    /// Задачи
    /// </summary>
    public class TasksService
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// GUID заведения
        /// </summary>
        public Guid GuidCreated { get; set; }

        /// <summary>
        /// Дата и время создания заявки
        /// </summary>
        public DateTime Create { get; set; }

        /// <summary>
        /// Статус заявки
        /// </summary>
        public StatusTask StatusTask { get; set; }

        /// <summary>
        /// Дата и время изменения заявки
        /// </summary>
        public DateTime DataTimeСhanges { get; set; }
    }

    public enum StatusTask
    {
        created,
        running,
        finished,
    }
}
