using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsWebApi.Models
{
    public class Top15NewsModel
    {
        [Key]
        [ForeignKey(nameof(NewModel))]
        public int NewModelId { get; set; }
        public NewModel NewModel { get; set; }

        public int rank { get; set; }
    }
}
