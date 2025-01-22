using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.AdminDTO
{
    public class AdminModelDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "الاسم مطلوب") , StringLength(maximumLength:200 , MinimumLength =5 , ErrorMessage = "الاسم يجب علي الاقل 5 حروف")]
        public string Name { get; set; }
        [Required(ErrorMessage = "الايميل مطلوب")]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "الايميل غير فعال")]
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public AdminModelDTO Clone()
        {
            return new AdminModelDTO { Id = Id, Name = Name, Email = Email, Roles = new List<string>() };
        }
    }
}
