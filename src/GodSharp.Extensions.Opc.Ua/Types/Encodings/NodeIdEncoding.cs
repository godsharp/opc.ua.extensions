using Opc.Ua;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class NodeIdEncoding : Encoding<NodeId>
    {
        protected override NodeId OnRead(IDecoder decoder, string name) => decoder.ReadNodeId(name);

        protected override void OnWrite(IEncoder encoder, NodeId field, string name) => encoder.WriteNodeId(name, field);
    }

    public sealed class NodeIdArrayEncoding : Encoding<NodeId[]>
    {
        protected override NodeId[] OnRead(IDecoder decoder, string name) => decoder.ReadNodeIdArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, NodeId[] field, string name) => encoder.WriteNodeIdArray(name, field);
    }
}
