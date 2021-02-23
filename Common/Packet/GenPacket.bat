START ../../PacketGenerator/bin/PacketGenerator.exe ../../PacketGenerator/PDL.xml
DEL  ..\..\DummyClient\Packet\GenPackets.cs
DEL  ..\..\DummyClient\Packet\ClientPacketManager.cs
DEL  ..\..\Server\Packet\GenPackets.cs
DEL  ..\..\Server\Packet\ServerPacketManager.cs
DEL  ..\..\Client\Assets\Scripts\Network\Packet\GenPackets.cs
DEL  ..\..\Client\Assets\Scripts\Network\Packet\\ClientPacketManager.cs
XCOPY /Y /S /D GenPackets.cs "..\..\DummyClient\Packet"
XCOPY /Y /S /D GenPackets.cs "..\..\Server\Packet"
XCOPY /Y /S /D GenPackets.cs "..\..\Client\Assets\Scripts\Network\Packet"
XCOPY /Y /S /D ClientPacketManager.cs "..\..\DummyClient\Packet"
XCOPY /Y /S /D ServerPacketManager.cs "..\..\Server\Packet"
XCOPY /Y /S /D ClientPacketManager.cs "..\..\Client\Assets\Scripts\Network\Packet"
