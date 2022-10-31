using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Engine.BO.CallCenter
{
    public class Call : BaseBO
    {
        public DateTime Received { get; set; }
        public DateTime? Answered { get; set; }
        public DateTime? Ended { get; set; }
        
        public TimeSpan? Duration => Answered != null? Received - Answered : null;
        public TimeSpan? Waiting =>  Answered != null && Ended != null? Answered - Ended : null;

        //public Session? Session { get; set; }

        public string? PhoneNumber { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusCall Status { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusCallEnd StatusEnd { get; set; }
        
    }
}
