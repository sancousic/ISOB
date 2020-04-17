using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ISOB_2
{
    class Serilizer <T>
    {
        public static byte[] Serilize(T source)
        {
            var json = JsonSerializer.Serialize<T>(source);
            return Encoding.UTF8.GetBytes(json);
        }
        public static T Deserilaze(byte[] source)
        {
            var json = Encoding.UTF8.GetString(source);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
