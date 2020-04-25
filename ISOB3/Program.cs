using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISOB3
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.Listen.Start();
            Hacker1.Hack.Start();
            Console.ReadLine();
        }
    }
}
