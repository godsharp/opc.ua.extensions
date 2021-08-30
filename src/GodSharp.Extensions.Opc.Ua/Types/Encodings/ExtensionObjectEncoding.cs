using Opc.Ua;

using System;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class ExtensionObjectEncoding : Encoding<ExtensionObject>
    {
        protected override ExtensionObject OnRead(IDecoder decoder, string name) => decoder.ReadExtensionObject(name);

        protected override void OnWrite(IEncoder encoder, ExtensionObject field, string name) => encoder.WriteExtensionObject(name, field);
    }

    public sealed class ExtensionObjectArrayEncoding : Encoding<ExtensionObject[]>
    {
        protected override ExtensionObject[] OnRead(IDecoder decoder, string name) => decoder.ReadExtensionObjectArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, ExtensionObject[] field, string name) => encoder.WriteExtensionObjectArray(name, field);
    }
}
