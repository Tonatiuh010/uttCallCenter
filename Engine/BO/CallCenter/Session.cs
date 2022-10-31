using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BO.CallCenter
{
    public class Session : BaseBO
    {
        public DateTime LoginDate { get; set; }
        public DateTime? LogoutDate { get; set; }

        public TimeSpan TimeLogged => LoginDate - DateTime.Now;
        public TimeSpan? TimeCall => CurrentCall != null ? CurrentCall.Answered - DateTime.Now : null;

        public Agent Agent { get; set; }
        public Station Station { get; set; }

        public Call? CurrentCall { get; set; }
        public bool IsActive { get; set; }

    }
}
