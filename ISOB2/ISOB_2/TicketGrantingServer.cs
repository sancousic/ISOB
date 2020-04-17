using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ISOB_2
{
    class TicketGrantingServer
    {
        public void Listen()
        {
            UdpClient reciever = new UdpClient(Config.TGS_port);
            IPEndPoint remoteIP = null;
            try
            {
                while (true)
                {
                    byte[] data = reciever.Receive(ref remoteIP);
                    remoteIP.Port = Config.C_port;

                    var message = Serilizer<Message>.Deserilaze(data);

                    if (message.Type == MessageType.СToTgs)
                    {
                        var tgt_json = Helper.RecoverData(
                            new List<byte>(DES.Decrypt(message.Data[0].ToArray(), Config.K_AS_TGS)));
                        var tgt = Serilizer<TicketGranting>.Deserilaze(tgt_json);

                        var aut1_json = Helper.RecoverData(
                            new List<byte>(DES.Decrypt(message.Data[1].ToArray(), Config.K_C_TGS)));
                        var a = Helper.ToString(aut1_json);

                        var aut1 = Serilizer<TimeMark>.Deserilaze(aut1_json);

                        var ID = Helper.ToString(message.Data[2].ToArray());

                        Message ReMessage = new Message();

                        if (tgt.ClientIdentity == aut1.C)
                        {
                            if (Helper.CheckTime(tgt.IssuingTime, aut1.T, tgt.Duration))
                            {
                                ReMessage.Type = MessageType.TgsToC;
                                var TGS = new TicketGranting()
                                {
                                    ClientIdentity = aut1.C,
                                    ServiceIdentity = ID,
                                    Duration = Config.TGSTicketDuration.Ticks,
                                    IssuingTime = DateTime.Now,
                                    Key = Helper.ToString(Config.K_C_SS)
                                };
                                var ticket_bytes = Helper.ExtendData(Serilizer<TicketGranting>.Serilize(TGS));
                                var k_c_ss_bytes = Helper.ExtendData(Config.K_C_SS);

                                var tb_enc = DES.Encrypt(ticket_bytes, Config.K_TGS_SS);
                                tb_enc = DES.Encrypt(tb_enc, Config.K_C_TGS);

                                var k_c_ss_enc = DES.Encrypt(k_c_ss_bytes, Config.K_C_TGS);

                                ReMessage.Data.Add(new List<byte>(tb_enc));
                                ReMessage.Data.Add(new List<byte>(k_c_ss_enc));
                            }
                            else
                            {
                                ReMessage.Type = MessageType.TicketNotValid;
                            }
                        }
                        else
                        {
                            ReMessage.Type = MessageType.AccessDenied;
                        }
                        ReMessage.Send(remoteIP);
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
