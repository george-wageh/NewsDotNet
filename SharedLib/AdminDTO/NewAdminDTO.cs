using SharedLib.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.AdminDTO
{
    public class NewAdminDTO:NewDTO
    {
        public bool IsInTopNews { get; set; }
    }
}
