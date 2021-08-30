
using CommonTest;

using System;
using System.IO;
using System.Linq;

namespace OpcUaTest
{
    public class UaAnsiCServerRunner : OpcUaTestRunner
    {
        public UaAnsiCServerRunner(OpcUaSession client, Action<string> output) : base(client, output)
        {
        }

        public override void Run()
        {
            Bool("ns=4;s=Demo.Static.Scalar.Boolean");

            Byte("ns=4;s=Demo.Static.Scalar.Byte");
            Short("ns=4;s=Demo.Static.Scalar.Int16");
            Int("ns=4;s=Demo.Static.Scalar.Int32");
            Long("ns=4;s=Demo.Static.Scalar.Int64");

            SByte("ns=4;s=Demo.Static.Scalar.SByte");
            UShort("ns=4;s=Demo.Static.Scalar.UInt16");
            UInt("ns=4;s=Demo.Static.Scalar.UInt32");
            ULong("ns=4;s=Demo.Static.Scalar.UInt64");

            Float("ns=4;s=Demo.Static.Scalar.Float");

            Double("ns=4;s=Demo.Static.Scalar.Double");

            String("ns=4;s=Demo.Static.Scalar.String");

            Run(
                GetRandomArray(r => (byte)r.Next(byte.MinValue, byte.MaxValue + 1), 8),
                "ns=4;s=Demo.Static.Scalar.ByteString",
                (s1, s2) => Enumerable.SequenceEqual(s1, s2),
                x => string.Join(" ", x.Select(s => s.ToString("x2")))
             );

            //DateTime("ns=4;s=Demo.Static.Scalar.DateTime");
            DateTimeUtc("ns=4;s=Demo.Static.Scalar.UtcTime");

            Guid("ns=4;s=Demo.Static.Scalar.Guid");

            RunEncodeableObject(new UaAnsiVector()
            {
                X = Random.NextDouble(),
                Y = Random.NextDouble(),
                Z = Random.NextDouble()
            },
                "ns=4;s=Demo.Static.Scalar.Vector",
                x => $"X:{x?.X},Y:{x?.Y},X:{x?.Z}",
                (x1, x2) => x1.X == x2.X && x1.Y == x2.Y && x1.Z == x2.Z
             );

            RunEncodeableObject(new UaAnsiUnion()
            {
                SwitchField = (uint)Random.Next(0, 3),
                Int32 = Random.Next(short.MinValue, short.MaxValue),
                String = Path.GetRandomFileName()
            },
                "ns=4;s=Demo.Static.Scalar.Union",
                x => $"SwitchField:{x?.SwitchField},Int32:{x?.Int32},String:{x?.String}",
                (x1, x2) =>
                {
                    if (x2 == null) return false;
                    if (x1.SwitchField != x2.SwitchField) return false;

                    return x1.SwitchField switch
                    {
                        0 => true,
                        1 => x1.Int32 == x2.Int32,
                        2 => x1.String == x2.String,
                        _ => false
                    };
                }
             );

            RunEncodeableObject(
                new WorkOrder
                {
                    Id = System.Guid.NewGuid(),
                    AssetId = Path.GetRandomFileName(),
                    StartTime = System.DateTime.UtcNow,
                    StatusComments = new[]
                    {
                        new WorkOrderStatus
                        {
                            Actor = Path.GetRandomFileName(),
                            Timestamp = System.DateTime.UtcNow,
                            Comment = Path.GetRandomFileName()
                        }
                    }
                },
                "ns=4;s=Demo.Static.Scalar.WorkOrder",
                x => $"Id:{x.Id},AssetId:{x.AssetId},StartTime:{x.StartTime},StatusComments:[{(!(x.StatusComments?.Length>0) ?"": string.Join(",",x.StatusComments.Select(s=> ($"{{Actor:{s.Actor},Timestamp:{s.Timestamp},Comment:{s.Comment}}}]"))))}]",
                (x1, x2) =>
                {
                    if (x2 == null) return false;
                    var ret = x1.Id == x2.Id && x1.AssetId == x2.AssetId && x1.StartTime == x2.StartTime;
                    if (!ret) return false;

                    if (x1.StatusComments?.Length != x2.StatusComments?.Length) return false;
                    if (x1.StatusComments?.Length == 0) return true;

                    for (int i = 0; i < x1.StatusComments.Length; i++)
                    {
                        if (
                            x1.StatusComments[i].Actor == x2.StatusComments[i].Actor &&
                            x1.StatusComments[i].Timestamp == x2.StatusComments[i].Timestamp &&
                            x1.StatusComments[i].Comment == x2.StatusComments[i].Comment
                        )
                        {
                            return false;
                        }
                    }

                    return true;
                }
             );

        }
    }
}
