using Opc.Ua;

using System;
using System.Linq;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class GuidEncoding : Encoding<Guid>
    {
        protected override Guid OnRead(IDecoder decoder, string name) =>decoder.ReadGuid(name);

        protected override void OnWrite(IEncoder encoder, Guid field, string name) => encoder.WriteGuid(name, field);
    }

    public sealed class GuidArrayEncoding : Encoding<Guid[]>
    {
        protected override Guid[] OnRead(IDecoder decoder, string name) => decoder.ReadGuidArray(name)?.Select(x => new Guid(x.GuidString)).ToArray();

        protected override void OnWrite(IEncoder encoder, Guid[] field, string name) => encoder.WriteGuidArray(name, field);
    }
}
