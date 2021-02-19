using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

using ServerCore;

namespace DummyClient
{
    

    class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            PlayerInfoReq packet = new PlayerInfoReq() { playerId = 1001, name="wonAdam" };
            packet.skills.Add(new PlayerInfoReq.Skill() { id = 101, level = 1, duration = 3.0f });
            packet.skills.Add(new PlayerInfoReq.Skill() { id = 102, level = 2, duration = 1.3f });
            packet.skills.Add(new PlayerInfoReq.Skill() { id = 103, level = 4, duration = 1.2f });
            packet.skills.Add(new PlayerInfoReq.Skill() { id = 104, level = 1, duration = 4.0f });
            packet.skills.Add(new PlayerInfoReq.Skill() { id = 105, level = 1, duration = 2.0f });

            // Send
            for (int i = 0; i < 5; i++)
            {
                ArraySegment<byte> sendBuff = packet.Write();

                if(sendBuff != null)
                    Send(sendBuff);
            }
        }

        public override void OnDisconnect(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Server] {recvData}");

            return buffer.Count;
        }

        public override void OnSend(int numOfByte)
        {
            Console.WriteLine($"Transfered Bytes: {numOfByte}");
        }
    }
}
