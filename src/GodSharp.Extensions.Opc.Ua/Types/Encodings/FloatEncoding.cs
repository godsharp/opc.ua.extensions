using Opc.Ua;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class FloatEncoding : Encoding<float>
    {
        protected override float OnRead(IDecoder decoder, string name) => decoder.ReadFloat(name);

        protected override void OnWrite(IEncoder encoder, float field, string name) => encoder.WriteFloat(name, field);
    }

    public sealed class FloatArrayEncoding : Encoding<float[]>
    {
        protected override float[] OnRead(IDecoder decoder, string name) => decoder.ReadFloatArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, float[] field, string name) => encoder.WriteFloatArray(name, field);
    }
}
