using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.DTO
{
    public class ResponseListNewsDTO:ResponseDTO<IEnumerable<NewDTO>>
    {
        public int PagesCount { get; set; }
        public string SectionName { get; set; }
    }
}
