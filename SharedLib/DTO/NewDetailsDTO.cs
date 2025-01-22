using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.DTO
{
    public class NewDetailsDTO:NewDTO
    {
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public int SectionId { get; set; }


    }
}
