using Opc.Ua;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class ShortEncoding : Encoding<short>
    {
        protected override short OnRead(IDecoder decoder, string name) => decoder.ReadInt16(name);

        protected override void OnWrite(IEncoder encoder, short field, string name) => encoder.WriteInt16(name, field);
    }

    public sealed class ShortArrayEncoding : Encoding<short[]>
    {
        protected override short[] OnRead(IDecoder decoder, string name) => decoder.ReadInt16Array(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, short[] field, string name) => encoder.WriteInt16Array(name, field);
    }

    public sealed class UnsignedShortEncoding : Encoding<ushort>
    {
        protected override ushort OnRead(IDecoder decoder, string name) => decoder.ReadUInt16(name);

        protected override void OnWrite(IEncoder encoder, ushort field, string name) => encoder.WriteUInt16(name, field);
    }

    public sealed class UnsignedShortArrayEncoding : Encoding<ushort[]>
    {
        protected override ushort[] OnRead(IDecoder decoder, string name) => decoder.ReadUInt16Array(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, ushort[] field, string name) => encoder.WriteUInt16Array(name, field);
    }
}
