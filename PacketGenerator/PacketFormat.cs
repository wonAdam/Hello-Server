using System;
using System.Collections.Generic;
using System.Text;

namespace PacketGenerator
{
    class PacketFormat
    {
        // {0} 패킷 이름
        // {1} 멤버 변수
        // {2} 멤버 변수 Read
        // {3} 멤버 변수 Write
        public static string packetFormat =
@"
class {0}
{{
    {1}

    public struct SkillInfo
    {{
        public int id;
        public short level;
        public float duration;

        public bool Write(Span<byte> s, ref ushort count)
        {{
            bool success = true;
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), id);
            count += sizeof(int);
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), level);
            count += sizeof(short);
            success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), duration);
            count += sizeof(float);

            return success;
        }}

        public void Read(ReadOnlySpan<byte> s, ref ushort count)
        {{
            id = BitConverter.ToInt32(s.Slice(count, s.Length - count));
            count += sizeof(int);
            level = BitConverter.ToInt16(s.Slice(count, s.Length - count));
            count += sizeof(short);
            duration = BitConverter.ToSingle(s.Slice(count, s.Length - count));
            count += sizeof(float);
        }}
    }}
    public List<SkillInfo> skills = new List<SkillInfo>();


    public void Read(ArraySegment<byte> segment)
    {{
        ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);

        ushort count = 0;

        ushort size = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
        count += sizeof(ushort);

        if (size != s.Length)
            return;

        ushort id = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
        count += sizeof(ushort);

        {2}

    }}

    public ArraySegment<byte> Write()
    {{
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);

        bool success = true;
        ushort count = 0;

        count += sizeof(ushort); // 패킷의 사이즈는 나중에 정함
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)PacketID.{0});
        count += sizeof(ushort);
        
        {3}

        // 패킷사이즈
        success &= BitConverter.TryWriteBytes(s, count);

        if (!success)
            return null;

        return SendBufferHelper.Close(count);
    }}
}}
";
        // {0} 변수 형식
        // {1} 변수 이름
        public static string memberFormat =
@"public {0} {1}";

        // {0} 변수 이름
        // {1} To~ 변수형식
        // {2} 변수 형식
        public static string readFormat =
@"this.{0} = BitConverter.{1}(s.Slice(count, s.Length - count));
count += sizeof({2});
";

        // {0} 변수 이름
        public static string readStringFormat =
@"ushort nameLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
count += sizeof(ushort);
name = Encoding.Unicode.GetString(s.Slice(count, {0}Len));
count += {0}Len;
";

        // {0} 변수 이름
        // {1} 변수 형식
        public static string writeFormat =
@"success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), this.{0});
count += sizeof({1});
";

        // {0} 변수 이름
        public static string writeStringFormat =
@"ushort {0}Len = (ushort)Encoding.Unicode.GetBytes({0}, 0, {0}.Length, segment.Array, segment.Offset + count + sizeof(ushort));
success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), {0}Len);
count += sizeof(ushort);
count += {0}Len;
";

    }
}
