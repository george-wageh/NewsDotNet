using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.DTO
{
    public class ResponseDTO<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static explicit operator ResponseDTO<T>(ResponseDTO<int> v)
        {
            throw new NotImplementedException();
        }
    }
}
