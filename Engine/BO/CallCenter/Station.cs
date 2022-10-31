using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.CallCenter
{
    public class Station : BaseBO
    {
        public int Row { get; set; }
        public int Desk { get; set; }
        public string? IPAddress { get; set; }
        public bool IsActive { get; set; }
    }
}
