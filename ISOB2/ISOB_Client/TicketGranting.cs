using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ISOB_Client
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
