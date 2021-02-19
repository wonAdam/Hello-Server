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
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            Thread.Sleep(5000);
            Disconnect();

            Console.WriteLine($"OnConnected : {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
            // test Send
            for (int i = 0; i < 5; i++)
            {
                S_Test testPacket = new S_Test() { testInt = i };
                ArraySegment<byte> sendBuff = testPacket.Write();

                if (sendBuff != null)
                    Send(sendBuff);
            }
        }

        public override void OnDisconnect(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }


        public override void OnSend(int numOfByte)
        {
            Console.WriteLine($"Transfered Bytes: {numOfByte}");
        }
    }
}
