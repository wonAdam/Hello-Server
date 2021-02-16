using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DummyClient
{
    class Packet
    {
        public ushort size;
        public ushort packetId;
    }

    class PlayerInfoReq : Packet
    {
        public long playerId;
    
    }

    class PlayerInfoOk : Packet
    {
        public int hp;
        public int attack;
    }

    public enum PacketID 
    {
        PlayerInfoReq = 1,
        PlayerInfoOk = 2,
    }

    class ServerSession : Session
    {
        //static unsafe void ToByte(byte[] array, int offset, ulong value)
        //{
        //    fixed (byte* ptr = &array[offset])
        //        *(ulong*)ptr = value;
        //}

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            PlayerInfoReq packet = new PlayerInfoReq() { packetId = (ushort)PacketID.PlayerInfoReq, playerId = 1001 };

            // Send
            //for (int i = 0; i < 5; i++)
            {
                ArraySegment<byte> s = SendBufferHelper.Open(4096);

                bool success = true;
                ushort count = 0;

                //success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), packet.size);
                count += 2; // 패킷의 사이즈는 나중에 정함
                success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), packet.packetId);
                count += 2;
                success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + count, s.Count - count), packet.playerId);
                count += 8;

                // 패킷의 사이즈
                success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset, s.Count), count);


                // 이런 방식은 계속 메모리를 할당하는 방법 => 안전하지만 메모리 낭비 & 속도 저하
                //byte[] size = BitConverter.GetBytes(packet.size);           // 2
                //byte[] packetId = BitConverter.GetBytes(packet.packetId);   // 2
                //byte[] playerId = BitConverter.GetBytes(packet.playerId);   // 8

                //Array.Copy(size, 0, s.Array, s.Offset + count, 2);
                //count += 2;
                //Array.Copy(packetId, 0, s.Array, s.Offset + count, 2);
                //count += 2;
                //Array.Copy(playerId, 0, s.Array, s.Offset + count, 8);
                //count += 8;

                ArraySegment<byte> sendBuff = SendBufferHelper.Close(count);

                if(success)
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
