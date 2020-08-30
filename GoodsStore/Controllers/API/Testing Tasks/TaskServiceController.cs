using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodsStore.Models;
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
        public async Task<ActionResult<TasksService>> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = await _context.TasksService.FirstOrDefaultAsync(r => r.GuidCreated == id);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        /// <summary>
        /// Добавить значение value
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _context.APIDatas.Add(new APIData()
            {
                Value = value,
                Value2 = value,
            });

            _context.SaveChangesAsync();
        }

        /// <summary>
        /// Чего то обновить
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] APIData data)
        {
            var api = await _context.APIDatas.FirstOrDefaultAsync(m => m.Id == id);
            if (api == null)
            {
                return NotFound();
            }

            api.Value = data.Value;
            api.Value2 = data.Value2;

            _context.APIDatas.Update(api);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Удалить элемент id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var api = await _context.APIDatas.FirstOrDefaultAsync(m => m.Id == id);
            if (api == null)
            {
                return NotFound();
            }

            _context.APIDatas.Remove(api);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
