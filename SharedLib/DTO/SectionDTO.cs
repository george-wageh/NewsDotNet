﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.DTO
{
    public class SectionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SectionDTO Clone() { 
            return new SectionDTO { Id = Id, Name = Name };
        }
    }

}
