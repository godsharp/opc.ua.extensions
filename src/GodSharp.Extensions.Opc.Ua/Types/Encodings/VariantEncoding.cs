using Opc.Ua;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class VariantEncoding : Encoding<Variant>
    {
        protected override Variant OnRead(IDecoder decoder, string name) => decoder.ReadVariant(name);

        protected override void OnWrite(IEncoder encoder, Variant field, string name) => encoder.WriteVariant(name, field);
    }

    public sealed class VariantArrayEncoding : Encoding<Variant[]>
    {
        protected override Variant[] OnRead(IDecoder decoder, string name) => decoder.ReadVariantArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, Variant[] field, string name) => encoder.WriteVariantArray(name, field);
    }
}
