using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISOB_Client
{
    class Client
    {
        public string Login { get; set; }
        private byte[] TicketGrantingTicket { get; set; }
        private byte[] TicketGrantingService { get; set; }
        private byte[] K_C_TGS { get; set; }
        private byte[] K_C_SS { get; set; }
        DateTime T4 { get; set; }
        TextBox _textBox;
        public Client(TextBox textBox)
        {
            _textBox = textBox;
        }

        private readonly IPEndPoint ASEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Config.AS_port);
        private readonly IPEndPoint SSEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Config.SS_port);
        private readonly IPEndPoint TGSEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Config.TGS_port);
        public void Register(string login)
        {
            Login = login;
            Message message = new Message(MessageType.CToAs);
            message.Data.Add(new List<byte>(Helper.StrToByteArray(login)));
            message.Send(ASEndPoint);
        }
        public void GetRes()
        {
            if (K_C_TGS != null &&
                TicketGrantingTicket != null)
            {
                Message message = new Message(MessageType.СToTgs);
                message.Data.Add(new List<byte>(TicketGrantingTicket));

                TimeMark mark = new TimeMark() { C = Login, T = DateTime.Now };
                var Aut1 = Helper.ExtendData(Serilazer<TimeMark>.Serilize(mark));
                message.Data.Add(new List<byte>(DES.Encrypt(Aut1, K_C_TGS)));
                message.Data.Add(new List<byte>(Helper.StrToByteArray(Config.SS_ID)));

                message.Send(TGSEndPoint);
            }
        }
        public void Listen()
        {
            UdpClient reciever = new UdpClient(Config.C_port);
            IPEndPoint remoteIP = null;
            try
            {
                while (true)
                {
                    byte[] data = reciever.Receive(ref remoteIP);
                    Message message = Serilazer<Message>.Deserilaze(data);
                    switch (message.Type)
                    {
                        case MessageType.AsToC:
                            TicketGrantingTicket = DES.Decrypt(message.Data[0].ToArray(), Config.K_C);
                            K_C_TGS = Helper.RecoverData(new List<byte>(DES.Decrypt(message.Data[1].ToArray(), Config.K_C)));
                            var a = Helper.ToString(K_C_TGS);
                            Print("AS to C complete!");
                            break;
                        case MessageType.TgsToC:
                            TicketGrantingService = DES.Decrypt(message.Data[0].ToArray(), K_C_TGS);
                            K_C_SS = Helper.RecoverData(new List<byte>(DES.Decrypt(message.Data[1].ToArray(), K_C_TGS)));

                            Message msg = new Message(MessageType.CToSs);
                            msg.Data.Add(new List<byte>(TicketGrantingService));

                            var mark = new TimeMark() { C = Login, T = DateTime.Now };
                            var Aut2 = Helper.ExtendData(Serilazer<TimeMark>.Serilize(mark));
                            T4 = mark.T;
                            msg.Data.Add(new List<byte>(DES.Encrypt(Aut2, K_C_SS)));

                            msg.Send(SSEndPoint);

                            Print("TGS to C complete");
                            break;
                        case MessageType.SsToC:
                            var t = DES.Decrypt(message.Data[0].ToArray(), K_C_SS);
                            var checkT_bytes = Helper.RecoverData(new List<byte>(t));
                            var asd = Helper.ToString(checkT_bytes);
                            var checkT = Serilazer<long>.Deserilaze(checkT_bytes);
                            if(T4.Ticks +1 == checkT)
                            {
                                Print("success");
                            }
                            break;
                        case MessageType.TicketNotValid:
                            Print("Ticket is not valid");
                            break;
                        case MessageType.AccessDenied:
                            Print("Access denied!");
                            break;
                        default:
                            MessageBox.Show("Invalid type of message");
                            break;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void Print(string message)
        {
            _textBox.Invoke(new Action(() =>
            {
                _textBox.AppendText(message + "\r\n");
            }));
        }
    }
}
