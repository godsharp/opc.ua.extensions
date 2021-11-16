using GodSharp.Extensions.Opc.Ua.Client;
using GodSharp.Extensions.Opc.Ua.Types.Encodings;

using Opc.Ua;

using System;
using CommonTest;
using Xunit;
using Xunit.Abstractions;
using Opc.Ua.Client;
using System.Linq;
using System.Reflection;
using System.IO;
using Opc.Ua.Client.ComplexTypes;
using GodSharp.Extensions.Opc.Ua.Types;

namespace OpcUaTest
{
    public class UaAnsiCServerUnitTest : UnitTestBase
    {
        private readonly string _server = "opc.tcp://GodSharp:48020";

        public UaAnsiCServerUnitTest(ITestOutputHelper outputHelper):base(outputHelper)
        {
        }

        [Fact]
        public void MainTest()
        {
            EncodingFactory.Instance.Register(new Assembly[]
            {
                typeof(IEncodingFactory).Assembly,
                typeof(WorkOrder).Assembly
            });

            // register type namespace
            EncodingFactory.Instance.RegisterTypeNamespace(
                new TypeNamespace()
                {
                    Type = typeof(UaAnsiVector).AssemblyQualifiedName,
                    TypeId = "nsu=http://www.unifiedautomation.com/DemoServer/;i=3002",
                    BinaryEncodingId = "nsu=http://www.unifiedautomation.com/DemoServer/;i=5054"
                }
            );

            //EncodingFactory.Instance.RegisterTypeNamespace(
            //    new TypeNamespace()
            //    {
            //        Type = typeof(UnionSwitchField).FullName,
            //        TypeId = "nsu=http://www.unifiedautomation.com/DemoServer/;i=3006",
            //        BinaryEncodingId = "nsu=http://www.unifiedautomation.com/DemoServer/;i=5003"
            //    }
            //);

            EncodingFactory.Instance.RegisterEncodeableTypes(Assembly.GetExecutingAssembly());

            var opc = new OpcUaSession(_server);
            opc.Connect();

            //new ComplexTypeSystem(opc.Session)?.Load().Wait();
            //OptionalFieldsReadWriteTest(opc.Session);
            //OptionSetReadWriteTest(opc.Session);
            //EnumArrayReadWriteTest(opc.Session);
            //EnumReadWriteTest(opc.Session);
            //LongArrayReadWriteTest(opc.Session);
            //GetObjectTypeTest(opc.Session);
            LongArrayReadWriteTest2(opc.Session);

            //new UaAnsiCServerRunner(opc, Output.WriteLine).Run();
            opc.Disconnect();
        }

        public void EnumReadWriteTest(Session session)
        {
            /*
                Low = 10,
                Normal = 40,
                High = 70,
                Urgent = 90,
                Immediate = 100
            */
            int[] array = new int[] { 10, 40, 70, 90, 100 };

            var node = "ns=4;s=Demo.Static.Scalar.Priority";
            var priority = (PriorityEnum)array[new Random((int)DateTime.Now.Ticks).Next(0, array.Length)];
            var ret = session.Write(node, priority);
            Output.WriteLine($"Write {node}={priority} {ret}");
            PriorityEnum _priority = session.Read<PriorityEnum>(node);

            Output.WriteLine($"Read {node}={_priority} {priority == _priority}");
            Assert.Equal(priority, _priority);
        }

        public void EnumArrayReadWriteTest(Session session)
        {
            var node = "ns=4;s=Demo.Static.Arrays.ServerState";
            int[] array = Enumerable.Range(0, 7).ToArray();
            var random = new Random((int)DateTime.Now.Ticks);
            var values = new ServerState[random.Next(3, 11)];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = (ServerState)array[random.Next(0, array.Length)];
            }

            var ret = session.Write(node, values);
            Output.WriteLine($"Write {node}={string.Join(",", values)} {ret}");
            ServerState[] _values = session.Read<ServerState[]>(node);
            Output.WriteLine($"Read {node}={string.Join(",", _values)} {values.SequenceEqual(_values)}");
            Assert.True(values.SequenceEqual(_values));
            Assert.Equal(values, _values);
        }

        public void LongArrayReadWriteTest(Session session)
        {
            var node = "ns=4;s=Demo.Static.Arrays.Int32";
            int[] array = Enumerable.Range(0, 7).ToArray();
            var random = new Random((int)DateTime.Now.Ticks);
            var values = new int[random.Next(3, 11)];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = random.Next(0, int.MaxValue);
            }

            var ret = session.Write(node, values);
            Output.WriteLine($"Write {node}={string.Join(",", values)} {ret}");
            var _values = session.Read<int[]>(node);
            Output.WriteLine($"Read {node}={string.Join(",", _values)} {values.SequenceEqual(_values)}");
            Assert.True(values.SequenceEqual(_values));
            Assert.Equal(values, _values);
        }

        public void LongArrayReadWriteTest2(Session session)
        {
            var node = "ns=4;s=Demo.Static.Arrays.Byte";
            var random = new Random((int)DateTime.Now.Ticks);

            byte b = (byte)random.Next(0, byte.MaxValue);
            int s = (byte)random.Next(2, 5);
            var values = Enumerable.Repeat(b, s).ToArray();
            bool ret;
            //var ret = session.Write(node, b, 0, s);
            //Output.WriteLine($"Write {node}={string.Join(",", values)} {ret}");
            //var _values = session.Read<byte>(node, 0, s);
            //Output.WriteLine($"Read {node}={string.Join(",", _values)} {values.SequenceEqual(_values)}");
            //Assert.True(values.SequenceEqual(_values));
            //Assert.Equal(values, _values);

            var c = random.Next(1, 10);
            for (int i = 0; i < c; i++)
            {
                b = (byte)random.Next(0, byte.MaxValue);
                var index = random.Next(0, 5);
                ret = session.Write(node, b, index);
                Output.WriteLine($"Write {node}[{index}]={b} {ret}");
                byte _b = session.Read<byte>(node, index, default);
                Output.WriteLine($"Read {node}=[{index}]={_b} {b == _b}");
                Assert.True(b == _b);
                Assert.Equal(b, _b);
            }
        }

        public void OptionalFieldsReadWriteTest(Session session)
        {
            var node = "ns=4;s=Demo.Static.Scalar.OptionalFields";

            var values = session.Read<OptionalFields>(node);
            Output.WriteLine($"Read {node}={values.EncodingMask}/{values.MandatoryInt32}/{values.OptionalInt32}/{values.MandatoryStringArray?.Length}/{values.OptionalStringArray?.Length}");

            var value = new OptionalFields();

            var random = new Random((int)DateTime.Now.Ticks);
            value.MandatoryInt32 = random.Next(0, 100);
            if (random.Next(0, 4) % 3 != 0)
            {
                value.OptionalInt32 = random.Next(0, 100);
                //value.EncodingMask |= 0b0001;
                value.WithOptionalField(x=>x.OptionalInt32);
            }

            if (random.Next(0, 4) % 3 != 1)
            {
                var r = random.Next(0, 5);
                if(r>0)
                {
                    value.OptionalStringArray = new string[r];
                    for (int i = 0; i < r; i++)
                    {
                        value.OptionalStringArray[i] = Path.GetRandomFileName();
                    }
                }
                //value.EncodingMask |= 0b0010;
                value.WithOptionalField(x => x.OptionalStringArray);
            }

            {
                var r = random.Next(0, 5);
                if (r > 0)
                {
                    value.MandatoryStringArray = new string[r];
                    for (int i = 0; i < r; i++)
                    {
                        value.MandatoryStringArray[i] = Path.GetRandomFileName();
                    }
                }
            }

            var ret = session.Write(node, value);
            Output.WriteLine($"Write {node}={value.EncodingMask}/{value.MandatoryInt32}/{value.OptionalInt32}/{value.MandatoryStringArray?.Length}/{value.OptionalStringArray?.Length} {ret}");
            var _values = session.Read<OptionalFields>(node);
            Output.WriteLine($"Read {node}={_values.EncodingMask}/{_values.MandatoryInt32}/{_values.OptionalInt32}/{_values.MandatoryStringArray?.Length}/{_values.OptionalStringArray?.Length} {value.EncodingMask == _values.EncodingMask && value.MandatoryInt32 == _values.MandatoryInt32 && value.OptionalInt32 == _values.OptionalInt32 && value.MandatoryStringArray?.Length == _values.MandatoryStringArray?.Length && value.OptionalStringArray?.Length == _values.OptionalStringArray?.Length}");

            //var ret = session.Write(node, values);
            //Output.WriteLine($"Write {node}={string.Join(",", values)} {ret}");
            //var _values = session.Read<int[]>(node);
            //Output.WriteLine($"Read {node}={string.Join(",", _values)} {values.SequenceEqual(_values)}");
            //Assert.True(values.SequenceEqual(_values));
            //Assert.Equal(values, _values);
        }

        public void OptionSetReadWriteTest(Session session)
        {
            var node = "ns=4;s=Demo.Static.Scalar.OptionSet";

            //(EnumDefinition)decoder.ReadEncodeable("EnumDefinition", typeof(EnumDefinition));

            var raw = session.Read<OptionSet>(node);
            
            //Output.WriteLine($"Read {node}={string.Join(",", raw.Fields.Select(x=>$"{x.Name}:{x.Value}"))}");
            //Output.WriteLine($"Read {node}={string.Join(",", (raw.Value as ExtensionObject).Body as byte[])}");
            //var values = session.Read<AccessRights>(node);
            //Output.WriteLine($"Read {node}={values.EncodingMask}/{values.MandatoryInt32}/{values.OptionalInt32}/{values.MandatoryStringArray?.Length}/{values.OptionalStringArray?.Length}");

            //var values = session.Read<AccessRights>(node);
            //Output.WriteLine($"Read {node}={values.EncodingMask}/{values.Read}/{values.Write}/{values.Execute}");

            //var value = new AccessRights();

            //var random = new Random((int)DateTime.Now.Ticks);
            //if (random.Next(0, 4) % 3 == 0)
            //{
            //    value.Read = random.Next(0, 4) % 3 == 0;
            //    value.EncodingMask |= 0b0001;
            //}
            //if (random.Next(0, 4) % 3 == 1)
            //{
            //    value.Write = random.Next(0, 4) % 3 == 1;
            //    value.EncodingMask |= 0b0010;
            //}
            //if (random.Next(0, 4) % 3 == 2)
            //{
            //    value.Execute = random.Next(0, 4) % 3 == 2;
            //    value.EncodingMask |= 0b0100;
            //}

            //var ret = session.Write(node, value);
            //Output.WriteLine($"Write {node}={value.EncodingMask}/{value.Read}/{value.Write}/{value.Execute} {ret}");
            //var _values = session.Read<AccessRights>(node);
            //Output.WriteLine($"Read {node}={_values.EncodingMask}/{_values.Read}/{_values.Write}/{_values.Execute} {value.EncodingMask==_values.EncodingMask && value.Read==_values.Read && value.Write==_values.Write&& value.Execute==_values.Execute}");

            //var ret = session.Write(node, values);
            //Output.WriteLine($"Write {node}={string.Join(",", values)} {ret}");
            //var _values = session.Read<int[]>(node);
            //Output.WriteLine($"Read {node}={string.Join(",", _values)} {values.SequenceEqual(_values)}");
            //Assert.True(values.SequenceEqual(_values));
            //Assert.Equal(values, _values);
        }

        public void GetObjectTypeTest(Session session)
        {
            var node = session.NodeCache.Find(Objects.RootFolder);

            var objects = session.Browse().FirstOrDefault(x => x.BrowseName == "Objects");

            var demo = session.Browse((NodeId)objects.NodeId).FirstOrDefault(x => x.BrowseName.Name.EndsWith("Demo"));
            var statics = session.Browse((NodeId)demo.NodeId).FirstOrDefault(x => x.BrowseName.Name.EndsWith("Static"));
            var scalar = session.Browse((NodeId)statics.NodeId).FirstOrDefault(x => x.BrowseName.Name.EndsWith("Scalar"));
            var structures = session.Browse((NodeId)scalar.NodeId).FirstOrDefault(x => x.BrowseName.Name.EndsWith("Structures"));
            var work = session.Browse((NodeId)structures.NodeId).FirstOrDefault(x => x.BrowseName.Name.EndsWith("WorkOrder"));

            //var node = session.ReadNode("ns=4;s=Demo.Static.Scalar.WorkOrder");
            var raw = session.Read(new[] { ExpandedNodeId.ToNodeId(work.NodeId,session.NamespaceUris) })?.FirstOrDefault();
            var ext = raw.Value as ExtensionObject;
            object val = ext.Body;
            object en = ext.Body as IEncodeable;
            var type = val.GetType();
            foreach (var property in type.GetProperties())
            {

            }
        }

        public void Test1(Session session)
        {
            var node = "ns=4;s=Demo.Static.Scalar.Vector";

            var random = new Random((int)DateTime.Now.Ticks);
            var vector = new UaAnsiVector
            {
                X = random.NextDouble() * 100,
                Y = random.NextDouble() * 100,
                Z = random.NextDouble() * 100
            };

            var ret = session.Write(node, vector);
            var _vector = session.Read<UaAnsiVector>(node);
            Assert.Equal(vector.X, _vector?.X);
            Assert.Equal(vector.Y, _vector?.Y);
            Assert.Equal(vector.Z, _vector?.Z);

            node = "ns=4;s=Demo.Static.Scalar.Union";

            var union = new UaAnsiUnion
            {
                SwitchField = 1,
                Int32 = random.Next(short.MinValue, short.MaxValue)
            };

            ret = session.Write(node, union);
            var _union = session.Read<UaAnsiUnion>(node);
            Assert.Equal(union.SwitchField, _union?.SwitchField);
            if (union.SwitchField == 1) Assert.Equal(union.Int32, _union?.Int32);
            if (union.SwitchField == 2) Assert.Equal(union.String, _union?.String);
        }
    }
}