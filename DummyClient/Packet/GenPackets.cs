using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using ServerCore;
public enum PacketID
{
    PlayerInfoReq = 1,	test = 2,	
}


class PlayerInfoReq
{
    public byte testByte;
	public long playerId;
	public string name;
	public class Skill
	        {
	            public int id;
		public short level;
		public float duration;
	
	            public void Read(ReadOnlySpan<byte> s, ref ushort count)
	            {
	                this.id = BitConverter.ToInt32(s.Slice(count, s.Length - count));
			count += sizeof(int);
			this.level = BitConverter.ToInt16(s.Slice(count, s.Length - count));
			count += sizeof(short);
			this.duration = BitConverter.ToSingle(s.Slice(count, s.Length - count));
			count += sizeof(float);
	            }
	
	            public bool Write(Span<byte> s, ref ushort count)
	            {
	                bool success = true;
	                success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.id);
			count += sizeof(int);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.level);
			count += sizeof(short);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.duration);
			count += sizeof(float);
	                return success;
	            }
	
	            
	        }
	        public List<Skill> skills = new List<Skill>();

    public void Read(ArraySegment<byte> segment)
    {
        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);

        ushort count = 0;

        ushort size = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
        count += sizeof(ushort);

        if (size != s.Length)
            return;

        ushort id = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
        count += sizeof(ushort);

        this.testByte = (byte)segment.Array[segment.Offset + count];
		count += sizeof(byte);
		this.playerId = BitConverter.ToInt64(s.Slice(count, s.Length - count));
		count += sizeof(long);
		ushort nameLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		name = Encoding.Unicode.GetString(s.Slice(count, nameLen));
		count += nameLen;
		this.skills.Clear();
		
		ushort skillLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		
		for (int i = 0; i < skillLen; i++)
		{
		    Skill skill = new Skill();
		    skill.Read(s, ref count);
		    this.skills.Add(skill);
		}

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);

        bool success = true;
        ushort count = 0;

        count += sizeof(ushort); // 패킷의 사이즈는 나중에 정함
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)PacketID.PlayerInfoReq);
        count += sizeof(ushort);
        
        segment.Array[segment.Offset + count] = (byte)this.testByte;
		count += sizeof(byte);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.playerId);
		count += sizeof(long);
		ushort nameLen = (ushort)Encoding.Unicode.GetBytes(name, 0, name.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), nameLen);
		count += sizeof(ushort);
		count += nameLen;
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)this.skills.Count);
		count += sizeof(ushort);
		foreach (Skill skill in this.skills)
		success &= skill.Write(s, ref count);

        // 패킷사이즈
        success &= BitConverter.TryWriteBytes(s, count);

        if (!success)
            return null;

        return SendBufferHelper.Close(count);
    }
}
class test
{
    public int testInt;

    public void Read(ArraySegment<byte> segment)
    {
        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);

        ushort count = 0;

        ushort size = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
        count += sizeof(ushort);

        if (size != s.Length)
            return;

        ushort id = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
        count += sizeof(ushort);

        this.testInt = BitConverter.ToInt32(s.Slice(count, s.Length - count));
		count += sizeof(int);

    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);

        bool success = true;
        ushort count = 0;

        count += sizeof(ushort); // 패킷의 사이즈는 나중에 정함
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)PacketID.test);
        count += sizeof(ushort);
        
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.testInt);
		count += sizeof(int);

        // 패킷사이즈
        success &= BitConverter.TryWriteBytes(s, count);

        if (!success)
            return null;

        return SendBufferHelper.Close(count);
    }
}