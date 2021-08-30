using Opc.Ua;

using System;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class QualifiedNameEncoding : Encoding<QualifiedName>
    {
        protected override QualifiedName OnRead(IDecoder decoder, string name) => decoder.ReadQualifiedName(name);

        protected override void OnWrite(IEncoder encoder, QualifiedName field, string name) => encoder.WriteQualifiedName(name, field);
    }

    public sealed class QualifiedNameArrayEncoding : Encoding<QualifiedName[]>
    {
        protected override QualifiedName[] OnRead(IDecoder decoder, string name) => decoder.ReadQualifiedNameArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, QualifiedName[] field, string name) => encoder.WriteQualifiedNameArray(name, field);
    }
}
