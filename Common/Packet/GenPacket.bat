START ../../PacketGenerator/bin/PacketGenerator.exe ../../PacketGenerator/PDL.xml
DEL  ..\..\DummyClient\Packet\GenPackets.cs
DEL  ..\..\DummyClient\Packet\ClientPacketManager.cs
DEL  ..\..\Server\Packet\GenPackets.cs
DEL  ..\..\Server\Packet\ServerPacketManager.cs
XCOPY /Y /S /D GenPackets.cs "..\..\DummyClient\Packet"
XCOPY /Y /S /D GenPackets.cs "..\..\Server\Packet"
XCOPY /Y /S /D ClientPacketManager.cs "..\..\DummyClient\Packet"
XCOPY /Y /S /D ServerPacketManager.cs "..\..\Server\Packet"