using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodsStore.Models;
using GoodsStore.Models.DTO.Task;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoodsStore.Controllers.API1
{
    /// <summary>
    /// Первый API контроллер
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("task")]
    [ApiExplorerSettings(GroupName = "TaskService")]
    public class TaskServiceController : ControllerBase
    {
        private readonly DataContextApp _context;

        public TaskServiceController(DataContextApp context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить значение по GUID
        /// </summary>
        /// <returns>Массив всех данных</returns>
        [HttpGet]
        [Route("/task/{id}")]
        public async Task<ActionResult<GetTask>> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = await _context.TasksServices.FirstOrDefaultAsync(r => r.GuidCreated == id);

            if (data == null)
            {
                return NotFound();
            }

            var dataDTO = (GetTask)data;

            return Ok(dataDTO);
        }

        /// <summary>
        /// Добавить значение value
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public async Task<ActionResult<Guid>> Post()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = _context.TasksServices.Add(new TasksService
            {
                StatusTask = StatusTask.created,
                GuidCreated = Guid.NewGuid(),
                TimeeStamp = DateTime.Now,
            });

            await _context.SaveChangesAsync();

            return Ok(data.Entity.GuidCreated);
        }
    }
}
