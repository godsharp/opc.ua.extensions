using Opc.Ua;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class LocalizedTextEncoding : Encoding<LocalizedText>
    {
        protected override LocalizedText OnRead(IDecoder decoder, string name) => decoder.ReadLocalizedText(name);

        protected override void OnWrite(IEncoder encoder, LocalizedText field, string name) => encoder.WriteLocalizedText(name, field);
    }

    public sealed class LocalizedTextArrayEncoding : Encoding<LocalizedText[]>
    {
        protected override LocalizedText[] OnRead(IDecoder decoder, string name) => decoder.ReadLocalizedTextArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, LocalizedText[] field, string name) => encoder.WriteLocalizedTextArray(name, field);
    }
}
