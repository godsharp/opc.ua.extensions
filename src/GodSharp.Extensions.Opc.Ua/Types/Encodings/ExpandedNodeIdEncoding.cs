using Opc.Ua;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class ExpandedNodeIdEncoding : Encoding<ExpandedNodeId>
    {
        protected override ExpandedNodeId OnRead(IDecoder decoder, string name) => decoder.ReadExpandedNodeId(name);

        protected override void OnWrite(IEncoder encoder, ExpandedNodeId field, string name) => encoder.WriteExpandedNodeId(name, field);
    }

    public sealed class ExpandedNodeIdArrayEncoding : Encoding<ExpandedNodeId[]>
    {
        protected override ExpandedNodeId[] OnRead(IDecoder decoder, string name) => decoder.ReadExpandedNodeIdArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, ExpandedNodeId[] field, string name) => encoder.WriteExpandedNodeIdArray(name, field);
    }
}
