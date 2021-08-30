using Opc.Ua;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    //public sealed class BooleanEncoding : Encoding<bool>
    //{
    //    protected override bool OnRead(IDecoder decoder, string name) => decoder.ReadBoolean(name);

    //    protected override void OnWrite(IEncoder encoder, bool field, string name) => encoder.WriteBoolean(name, field);
    //}

    //public sealed class BooleanArrayEncoding : Encoding<bool[]>
    //{
    //    protected override bool[] OnRead(IDecoder decoder, string name) => decoder.ReadBooleanArray(name)?.ToArray();

    //    protected override void OnWrite(IEncoder encoder, bool[] field, string name) => encoder.WriteBooleanArray(name, field);
    //}

    public sealed class BooleanEncoding : IEncoding<bool>
    {
        public void Read(IDecoder decoder, ref bool field, string name)
        {
            field = decoder.ReadBoolean(name);
        }

        public void Write(IEncoder encoder, bool field, string name)
        {
            encoder.WriteBoolean(name, field);
        }
    }

    public sealed class BooleanArrayEncoding : IEncoding<bool[]>
    {
        public void Read(IDecoder decoder, ref bool[] field, string name)
        {
            field = decoder.ReadBooleanArray(name)?.ToArray();
        }

        public void Write(IEncoder encoder, bool[] field, string name)
        {
            encoder.WriteBooleanArray(name, field);
        }
    }
}
