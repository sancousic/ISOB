using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ISOB_Client
{
    [Serializable]
    class Message
    {
        public Message(MessageType messageType = 0)
        {
            Type = messageType;
            Data = new List<List<byte>>();
        }
        public Message() { }
        public MessageType Type { get; set; }
        public List<List<byte>> Data { get; set; }
        public void Send(IPEndPoint remoteIP)
        {
            UdpClient sender = new UdpClient();

            try
            {
                byte[] dgram = Serilazer<Message>.Serilize(this);
                sender.Send(dgram, dgram.Length, remoteIP);
            }
            finally
            {
                sender.Close();
            }
        }
    }
}
