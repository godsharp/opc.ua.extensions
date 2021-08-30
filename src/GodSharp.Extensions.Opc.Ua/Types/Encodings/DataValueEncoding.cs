using Opc.Ua;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public sealed class DataValueEncoding : Encoding<DataValue>
    {
        protected override DataValue OnRead(IDecoder decoder, string name) => decoder.ReadDataValue(name);

        protected override void OnWrite(IEncoder encoder, DataValue field, string name) => encoder.WriteDataValue(name, field);
    }

    public sealed class DataValueArrayEncoding : Encoding<DataValue[]>
    {
        protected override DataValue[] OnRead(IDecoder decoder, string name) => decoder.ReadDataValueArray(name)?.ToArray();

        protected override void OnWrite(IEncoder encoder, DataValue[] field, string name) => encoder.WriteDataValueArray(name, field);
    }
}
