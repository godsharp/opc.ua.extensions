using Opc.Ua;

using System;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class StatusCodeEncoding : Encoding<StatusCode>
    {
        protected override StatusCode OnRead(IDecoder decoder, string name) => decoder.ReadStatusCode(name);

        protected override void OnWrite(IEncoder encoder, StatusCode field, string name) => encoder.WriteStatusCode(name, field);
    }

    public sealed class StatusCodeArrayEncoding : Encoding<StatusCode[]>
    {
        protected override StatusCode[] OnRead(IDecoder decoder, string name) => decoder.ReadStatusCodeArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, StatusCode[] field, string name) => encoder.WriteStatusCodeArray(name, field);
    }
}
