using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.DTO
{
    public class NewAddDTO
    {
        [Required(ErrorMessage ="")]
        public int Id { get; set; }
        [Required(ErrorMessage = "العنوان مطلوب")]
        public string Title { get; set; }
        [Required(ErrorMessage = "قم بادخال المقاله")]
        public string Description { get; set; }
        [Required(ErrorMessage = "تاكد من اختيار الصوره والضغط علي رفع الصوره")]
        public string Image { get; set; }
        [Required(ErrorMessage = "قم بادخال اسم الكاتب")]
        public string Author { get; set; }
        [Required(ErrorMessage = "اختار الفئه") , Range(1, int.MaxValue, ErrorMessage = "اختار الفئه")]
        public int SectionId { get; set; }
    }
}
