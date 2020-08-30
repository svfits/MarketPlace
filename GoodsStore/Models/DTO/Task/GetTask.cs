using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodsStore.Models.DTO.Task
{
    public class GetTask
    {
        public string Status { get; set; }

        public string TimeStamp { get; set; }

        public static explicit operator GetTask(TasksService v)
        {
            return new GetTask()
            {
                Status = Enum.GetName(typeof(StatusTask), v.StatusTask),
                TimeStamp = v.TimeeStamp.ToString("O"),
            };
        }
    }
}
