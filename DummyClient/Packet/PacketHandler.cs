using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

public class PacketHandler
{
    public static void S_ChatHandler(PacketSession session, IPacket packet)
    {
        S_Chat chatPacket = packet as S_Chat;
        ServerSession serverSession = session as ServerSession;

        Console.WriteLine($"S_Chat : playerId={chatPacket.playerId} / chat={chatPacket.chat}");
    }
}
