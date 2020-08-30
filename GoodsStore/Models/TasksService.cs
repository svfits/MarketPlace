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
        /// Task GUID 
        /// </summary>
        public Guid GuidCreated { get; set; }
        
        /// <summary>
        /// Статус заявки
        /// </summary>
        public StatusTask StatusTask { get; set; }

        /// <summary>
        /// Дата и время изменения заявки
        /// </summary>
        public DateTime TimeeStamp { get; set; }
    }

    public enum StatusTask
    {
        created,
        running,
        finished,
    }
}
