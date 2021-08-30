using CommonTest;

using Opc.Ua;

using System;
using System.IO;

namespace OpcUaTest
{
    public class ProsysOpcUaSimulatorRunner : OpcUaTestRunner
    {
        public ProsysOpcUaSimulatorRunner(OpcUaSession client, Action<string> output) : base(client, output)
        {
        }

        public override void Run()
        {
            Bool("ns=5;s=BooleanDataItem");

            Byte("ns=5;s=ByteDataItem");
            Short("ns=5;s=Int16DataItem");
            Int("ns=5;s=Int32DataItem");
            Long("ns=5;s=Int64DataItem");

            SByte("ns=5;s=SByteDataItem");
            UShort("ns=5;s=UInt16DataItem");
            UInt("ns=5;s=UInt32DataItem");
            ULong("ns=5;s=UInt64DataItem");

            Float("ns=5;s=FloatDataItem");

            Double("ns=5;s=DoubleDataItem");

            String("ns=5;s=StringDataItem");

            DateTime("ns=5;s=DateTimeDataItem");

            Guid("ns=5;s=GUIDDataItem");

            RunEncodeableObject(new V3()
            {
                X = Random.NextDouble(),
                Y = Random.NextDouble(),
                Z = Random.NextDouble()
            },
                "ns=3;i=1007",
                x => $"X:{x.X},Y:{x.Y},X:{x.Z}",
                (x1, x2) => x1.X == x2.X && x1.Y == x2.Y && x1.Z == x2.Z
             );
        }

        /// <summary>
        /// ThreeDVector
        /// </summary>
        public class V3 : EncodeableObject
        {
            public double X;
            public double Y;
            public double Z;

            public override ExpandedNodeId XmlEncodingId => NodeId.Null;
            public override ExpandedNodeId TypeId => ExpandedNodeId.Parse("nsu=http://opcfoundation.org/UA/;i=18808");
            public override ExpandedNodeId BinaryEncodingId => ExpandedNodeId.Parse("nsu=http://opcfoundation.org/UA/;i=18817");

            public V3() : base()
            {
            }

            public override void Encode(IEncoder encoder)
            {
                encoder.WriteDouble("X", this.X);
                encoder.WriteDouble("Y", this.Y);
                encoder.WriteDouble("Z", this.Z);
            }

            public override void Decode(IDecoder decoder)
            {
                X = decoder.ReadDouble("X");
                Y = decoder.ReadDouble("Y");
                Z = decoder.ReadDouble("Z");
            }

            public override string ToString() => $"{{ X={this.X}; Y={this.Y}; Z={this.Z}; }}";
        }
    }
}
