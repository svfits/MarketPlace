namespace GoodsStore.SchedulerTask
{
    public interface IBackgroundTask
    {
        /// <summary>
        /// Запустим в фоне выполнение
        /// </summary>
        /// <param name="Id">Идентификатор в БД</param>
        public void RunTask(int Id);
    }
}
