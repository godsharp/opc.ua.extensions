using Opc.Ua;
// ReSharper disable CheckNamespace

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class StringEncoding : Encoding<string>
    {
        protected override string OnRead(IDecoder decoder, string name) => decoder.ReadString(name);

        protected override void OnWrite(IEncoder encoder, string field, string name) => encoder.WriteString(name, field);
    }

    public sealed class StringArrayEncoding : Encoding<string[]>
    {
        protected override string[] OnRead(IDecoder decoder, string name) => decoder.ReadStringArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, string[] field, string name) => encoder.WriteStringArray(name, field);
    }
}
