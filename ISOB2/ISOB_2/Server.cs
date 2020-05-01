using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ISOB_2
{
    class Server
    {
        public void Listen()
        {
            Console.WriteLine($"SS started on 127.0.0.1:{Config.SS_port}");
            UdpClient reciever = new UdpClient(Config.SS_port);
            IPEndPoint remoteIP = null;
            try
            {
                while (true)
                {
                    byte[] data = reciever.Receive(ref remoteIP);
                    remoteIP.Port = Config.C_port;

                    var message = Serilizer<Message>.Deserilaze(data);
                    Console.WriteLine(message.ToString() + $"from {remoteIP.Address}:{remoteIP.Port};");
                    if (message.Type == MessageType.CToSs)
                    {
                        var tgs_bytes = Helper.RecoverData(
                            new List<byte>(DES.Decrypt(message.Data[0].ToArray(), Config.K_TGS_SS)));
                        var tgs = Serilizer<TicketGranting>.Deserilaze(tgs_bytes);

                        var aut2_bytes = Helper.RecoverData(
                            new List<byte>(DES.Decrypt(message.Data[1].ToArray(), Config.K_C_SS)));
                        var aut2 = Serilizer<TimeMark>.Deserilaze(aut2_bytes);

                        Message ReMessage = new Message();
                        if (Helper.CheckTime(tgs.IssuingTime, aut2.T, tgs.Duration))
                        {
                            ReMessage.Type = MessageType.SsToC;
                            DateTime reTime = aut2.T;
                            var time_bytes = Serilizer<long>.Serilize(aut2.T.Ticks + 1);
                            var bytes = DES.Encrypt(Helper.ExtendData(time_bytes),
                                                    Config.K_C_SS);
                            ReMessage.Data.Add(new List<byte>(bytes));
                        }
                        else ReMessage.Type = MessageType.TicketNotValid;

                        ReMessage.Send(remoteIP);
                        Console.WriteLine($"Message sended from SS to {remoteIP.Address}:{remoteIP.Port}");
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
