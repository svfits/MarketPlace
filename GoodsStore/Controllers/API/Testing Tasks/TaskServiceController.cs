using System;
using System.Threading.Tasks;
using GoodsStore.Models;
using GoodsStore.Models.DTO.Task;
using GoodsStore.SchedulerTask;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoodsStore.Controllers.TaskService
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

        public IBackgroundTask BackgroundTask { get; }

        public TaskServiceController(DataContextApp context, IBackgroundTask BackgroundTask)
        {
            _context = context;
            this.BackgroundTask = BackgroundTask;
        }

        /// <summary>
        /// Получить значение по GUID
        /// </summary>
        /// <param name="id">Индификатор записи</param>
        /// <returns></returns>
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
        /// Добавить новый Task
        /// </summary>
        /// <returns>GUID добавленой записи</returns>
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
            var guid = data.Entity.GuidCreated;

            var dataForChanged = await _context.TasksServices.FirstAsync(q => q.GuidCreated == guid);
            dataForChanged.StatusTask = StatusTask.running;
            await _context.SaveChangesAsync();

            BackgroundTask.RunTask(dataForChanged.Id);

            return StatusCode(202, guid);
        }
    }
}
