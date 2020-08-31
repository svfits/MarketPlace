using GoodsStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GoodsStore.SchedulerTask
{
    public class BackgroundTask : IBackgroundTask
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private const int PauseChangeMilSec = 120 * 1000;

        public BackgroundTask(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void RunTask(int Id)
        {
            Task.Run(() => TaskRun(Id));
        }

        private async void TaskRun(int id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContextApp>();

            Thread.Sleep(PauseChangeMilSec);

            var data = await db.TasksServices.FirstOrDefaultAsync(b => b.Id == id);
            data.StatusTask = StatusTask.finished;
            data.TimeeStamp = DateTime.Now;
            await db.SaveChangesAsync();
        }
    }
}
