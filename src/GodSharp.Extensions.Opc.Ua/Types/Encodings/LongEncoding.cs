using Opc.Ua;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class LongEncoding : Encoding<long>
    {
        protected override long OnRead(IDecoder decoder, string name) => decoder.ReadInt64(name);

        protected override void OnWrite(IEncoder encoder, long field, string name) => encoder.WriteInt64(name, field);
    }

    public sealed class LongArrayEncoding : Encoding<long[]>
    {
        protected override long[] OnRead(IDecoder decoder, string name) => decoder.ReadInt64Array(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, long[] field, string name) => encoder.WriteInt64Array(name, field);
    }

    public sealed class UnsignedLongEncoding : Encoding<ulong>
    {
        protected override ulong OnRead(IDecoder decoder, string name) => decoder.ReadUInt64(name);

        protected override void OnWrite(IEncoder encoder, ulong field, string name) => encoder.WriteUInt64(name, field);
    }

    public sealed class UnsignedLongArrayEncoding : Encoding<ulong[]>
    {
        protected override ulong[] OnRead(IDecoder decoder, string name) => decoder.ReadUInt64Array(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, ulong[] field, string name) => encoder.WriteUInt64Array(name, field);
    }
}
