using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

using ServerCore;

namespace Server
{
    class ClientSession : PacketSession
    {
        public int SessionId { get; set; }
        public GameRoom Room { get; set; }
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            Program.Room.Enter(this);

        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
            // test Send
            for (int i = 0; i < 5; i++)
            {
                S_Chat testPacket = new S_Chat() { chat = "dsadada", playerId = 1000 };
                ArraySegment<byte> sendBuff = testPacket.Write();

                if (sendBuff != null)
                    Send(sendBuff);
            }
        }

        public override void OnDisconnect(EndPoint endPoint)
        {
            SessionManager.Instance.Remove(this);
            if (Room != null)
            {
                Room.Exit(this);
                Room = null;
            }

            Console.WriteLine($"OnDisconnected : {endPoint}");
        }


        public override void OnSend(int numOfByte)
        {
            Console.WriteLine($"Transfered Bytes: {numOfByte}");
        }
    }
}
