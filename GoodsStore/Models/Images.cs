namespace GoodsStore.Models
{
    public class Images
    {
        public int Id { get; set; }

        /// <summary>
        /// Тело картинки
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Название картинки
        /// </summary>
        public string Name { get; set; }
    }
}