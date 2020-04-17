using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ISOB_2
{
    [Serializable]
    class TicketGranting
    {
        public TicketGranting() { }
        public string ClientIdentity { get; set; }
        public string ServiceIdentity { get; set; }
        public DateTime IssuingTime { get; set; }
        public long Duration { get; set; }
        public string Key { get; set; }
    }   
}
