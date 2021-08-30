using Opc.Ua;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class ByteEncoding : Encoding<byte>
    {
        protected override byte OnRead(IDecoder decoder, string name) => decoder.ReadByte(name);

        protected override void OnWrite(IEncoder encoder, byte field, string name) => encoder.WriteByte(name, field);
    }

    public sealed class ByteArrayEncoding : Encoding<byte[]>
    {
        protected override byte[] OnRead(IDecoder decoder, string name) => decoder.ReadByteArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, byte[] field, string name) => encoder.WriteByteArray(name, field);
    }

    public sealed class SignedByteEncoding : Encoding<sbyte>
    {
        protected override sbyte OnRead(IDecoder decoder, string name) => decoder.ReadSByte(name);

        protected override void OnWrite(IEncoder encoder, sbyte field, string name) => encoder.WriteSByte(name, field);
    }

    public sealed class SignedByteArrayEncoding : Encoding<sbyte[]>
    {
        protected override sbyte[] OnRead(IDecoder decoder, string name) => decoder.ReadSByteArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, sbyte[] field, string name) => encoder.WriteSByteArray(name, field);
    }
}
