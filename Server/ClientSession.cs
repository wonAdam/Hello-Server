using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace Server
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


    class ClientSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            //Packet packet = new Packet() { size = 4, packetId = 9 };

            //ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
            //byte[] buffer = BitConverter.GetBytes(packet.size);
            //byte[] buffer2 = BitConverter.GetBytes(packet.packetId);
            //Array.Copy(buffer, 0, openSegment.Array, openSegment.Offset, buffer.Length);
            //Array.Copy(buffer2, 0, openSegment.Array, openSegment.Offset + buffer.Length, buffer2.Length);
            //ArraySegment<byte> sendBuff = SendBufferHelper.Close(packet.size);

            //Send(sendBuff);
            Thread.Sleep(5000);
            Disconnect();

            Console.WriteLine($"OnConnected : {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {

            ushort count = 0;
            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;

            if (size != buffer.Count)
                return;

            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;

            switch((PacketID)id)
            {
                case PacketID.PlayerInfoReq:
                    {
                        long playerId = BitConverter.ToInt64(buffer.Array, buffer.Offset + count);
                        count += 8;
                        Console.WriteLine($"PlayerInfoReq : playerId={playerId}");
                    }
                    break;
                case PacketID.PlayerInfoOk:

                default:
                    break;
            }

            Console.WriteLine($"Recv Packet ID : {id} / size : {size}");
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
