using System;
using System.Collections.Generic;
using System.Text;

namespace ISOB_2
{
    public static class Config
    {
        public static readonly byte[] K_C = Helper.ExtendKey("K_C");
        public static readonly byte[] K_C_TGS = Helper.ExtendKey("K_C_TGS");
        public static readonly byte[] K_C_SS = Helper.ExtendKey("K_C_SS");
        public static readonly byte[] K_AS_TGS = Helper.ExtendKey("K_AS_TGS");
        public static readonly byte[] K_TGS_SS = Helper.ExtendKey("K_TGS_SS");
        
        public static readonly int C_port = 1233;
        public static readonly int AS_port = 1234;
        public static readonly int SS_port = 1235;
        public static readonly int TGS_port = 1236;

        public static readonly TimeSpan ASTicketDuration = new TimeSpan(24, 0, 0);
        public static readonly TimeSpan TGSTicketDuration = new TimeSpan(12, 0, 0);

        public static readonly string tgs = "tgs";
        public static readonly string SS_ID = "SS_ID";

    }
}
