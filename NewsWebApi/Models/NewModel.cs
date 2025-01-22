using System.ComponentModel.DataAnnotations.Schema;

namespace NewsWebApi.Models
{
    public class NewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }

        [ForeignKey(nameof(SectionModel))]
        public int SectionId { get; set; }
        public SectionModel SectionModel { get; set; }

        public string ImageUrl { get; set; }

        public DateTime Date { get; set; }

        public long ViewsCount { get; set; }

    }
}
