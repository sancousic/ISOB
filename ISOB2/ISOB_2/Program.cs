using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ISOB_2
{
    class Program
    {
        static void Main(string[] args)
        {
            AuthServer AS = new AuthServer();
            TicketGrantingServer TGS = new TicketGrantingServer();
            Server SS = new Server();
            try
            {
                Task.Run(() => AS.Listen());
                Task.Run(() => TGS.Listen());
                Task.Run(() => SS.Listen());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.ReadLine();
        }
    }
}
