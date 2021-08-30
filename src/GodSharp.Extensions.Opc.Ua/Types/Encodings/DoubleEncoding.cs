using Opc.Ua;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class DoubleEncoding : Encoding<double>
    {
        protected override double OnRead(IDecoder decoder, string name) => decoder.ReadDouble(name);

        protected override void OnWrite(IEncoder encoder, double field, string name) => encoder.WriteDouble(name, field);
    }

    public sealed class DoubleArrayEncoding : Encoding<double[]>
    {
        protected override double[] OnRead(IDecoder decoder, string name) => decoder.ReadDoubleArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, double[] field, string name) => encoder.WriteDoubleArray(name, field);
    }
}
