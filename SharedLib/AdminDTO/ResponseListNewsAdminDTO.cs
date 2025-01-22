using SharedLib.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.AdminDTO
{
    public class ResponseListNewsAdminDTO:ResponseDTO<IEnumerable<NewAdminDTO>>
    {
        public int PagesCount { get; set; }
        public string SectionName { get; set; }
    }
}
