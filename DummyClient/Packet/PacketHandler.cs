using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

public class PacketHandler
{

    public static void S_TestHandler(PacketSession session, IPacket packet)
    {
        S_Test p = packet as S_Test;

        Console.WriteLine($"test : testInt={p.testInt}");
        
    }
}
