using System;
using System.Collections.Generic;
using System.Text;

namespace ISOB_Client
{
    public static class Config
    {
        public static readonly byte[] K_C = Helper.ExtendKey("K_C");

        public static readonly int C_port = 1233;
        public static readonly int AS_port = 1234;
        public static readonly int SS_port = 1235;
        public static readonly int TGS_port = 1236;

        public static readonly string tgs = "tgs";
        public static readonly string SS_ID = "SS_ID";
    }
}
