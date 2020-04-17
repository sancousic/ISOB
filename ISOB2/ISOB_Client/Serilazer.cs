using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ISOB_Client
{
    class Serilazer<T>
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
