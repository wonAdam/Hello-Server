using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

public class PacketHandler
{
    public static void C_PlayerInfoReqHandler(PacketSession session, IPacket packet)
    {
        C_PlayerInfoReq p = packet as C_PlayerInfoReq;

        Console.WriteLine($"PlayerInfoReq : playerId={p.playerId} / playerName={p.name}");

        foreach (C_PlayerInfoReq.Skill skill in p.skills)
        {
            Console.WriteLine($"Skill : id={skill.id} / level={skill.level} / duration={skill.duration}");
        }
    }

}
