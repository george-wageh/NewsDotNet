using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.DTO
{
    public class NewsQueryDTO
    {
        public int PageNum { set; get; }
        public int SectionId { set; get; }
        public string QString { set; get; }
    }
}
