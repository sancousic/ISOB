using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ISOB_2
{
    class AuthServer
    {
        private List<string> Users = new List<string>();

        public AuthServer()
        {
            Users.Add("user");
            Users.Add("anonym");
            Users.Add("");
        }

        public void Listen()
        {
            UdpClient reciever = new UdpClient(Config.AS_port);
            Console.WriteLine($"AS started on 127.0.0.1:{Config.AS_port}");
            IPEndPoint remoteIP = null;
            try
            {
                while (true)
                {
                    byte[] data = reciever.Receive(ref remoteIP);
                    remoteIP.Port = Config.C_port;
                    Message ReMessage = new Message();

                    var message = Serilizer<Message>.Deserilaze(data);

                    if(message.Type == MessageType.CToAs)
                    {                        
                        var id = Helper.ToString(message.Data[0].ToArray());
                        Console.WriteLine($"Message from {remoteIP.Address}:{remoteIP.Port} with c = {id} to AuthServer!");
                        if (Users.Contains(id))
                        {
                            ReMessage.Type = MessageType.AsToC;

                            TicketGranting ticket = new TicketGranting()
                            {
                                ClientIdentity = id,
                                Duration = Config.ASTicketDuration.Ticks,
                                IssuingTime = DateTime.Now,
                                ServiceIdentity = Config.tgs,
                                Key = Helper.ToString(Config.K_C_TGS)
                            };
                            var ticket_bytes = Helper.ExtendData(Serilizer<TicketGranting>.Serilize(ticket));
                            var k_c_tgs_bytes = Helper.ExtendData(Config.K_C_TGS);

                            var tb_enc = DES.Encrypt(ticket_bytes, Config.K_AS_TGS);
                            tb_enc = DES.Encrypt(tb_enc, Config.K_C);

                            var k_c_tgs_enc = DES.Encrypt(k_c_tgs_bytes, Config.K_C);
                            
                            ReMessage.Data.Add(new List<byte>(tb_enc));
                            ReMessage.Data.Add(new List<byte>(k_c_tgs_enc));

                        }
                        else
                        {
                            ReMessage.Type = MessageType.AccessDenied;
                            Console.WriteLine("AccessDenied in AS");
                        }
                        ReMessage.Send(remoteIP);
                        Console.WriteLine($"Message sended from AS to {remoteIP.Address}:{remoteIP.Port}!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
