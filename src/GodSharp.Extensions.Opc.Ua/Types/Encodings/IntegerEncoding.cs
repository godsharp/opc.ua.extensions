using Opc.Ua;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class IntegerEncoding : Encoding<int>
    {
        protected override int OnRead(IDecoder decoder, string name) => decoder.ReadInt32(name);

        protected override void OnWrite(IEncoder encoder, int field, string name) => encoder.WriteInt32(name, field);
    }

    public sealed class IntegerArrayEncoding : Encoding<int[]>
    {
        protected override int[] OnRead(IDecoder decoder, string name) => decoder.ReadInt32Array(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, int[] field, string name) => encoder.WriteInt32Array(name, field);
    }

    public sealed class UnsignedIntegerEncoding : Encoding<uint>
    {
        protected override uint OnRead(IDecoder decoder, string name) => decoder.ReadUInt32(name);

        protected override void OnWrite(IEncoder encoder, uint field, string name) => encoder.WriteUInt32(name, field);
    }

    public sealed class UnsignedIntegerArrayEncoding : Encoding<uint[]>
    {
        protected override uint[] OnRead(IDecoder decoder, string name) => decoder.ReadUInt32Array(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, uint[] field, string name) => encoder.WriteUInt32Array(name, field);
    }
}
