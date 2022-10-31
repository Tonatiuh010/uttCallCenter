using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.CallCenter
{
    public class SessionLog : BaseBO
    {
        public Session Session { get; set; }
        public StatusSession Status { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public TimeSpan? Elapsed => End != null ? End - Start : null;

    }
}
