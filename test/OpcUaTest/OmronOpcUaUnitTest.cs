using GodSharp.Extensions.Opc.Ua.Client;
using Opc.Ua;
using System.Reflection;
using System;
using CommonTest;
using Xunit;
using GodSharp.Extensions.Opc.Ua.Types.Encodings;
using Opc.Ua.Client;
using System.Linq;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace OpcUaTest
{
    public partial class OmronOpcUaUnitTest : UnitTestBase
    {
        private readonly string _server = "opc.tcp://192.168.250.1:4840";

        public OmronOpcUaUnitTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public void MainTest()
        {
            EncodingFactory.Instance.Register(new Assembly[]
            { typeof(IEncodingFactory).Assembly,
                Assembly.GetEntryAssembly()
            });

            var opc = new OpcUaSession(_server);
            opc.Connect();
            ByteArrayReadWriteTest_With_Range_Values(opc.Session);
            ByteArrayReadWriteTest_With_Index_Range(opc.Session);
            ByteArrayReadWriteTest_With_Index_One(opc.Session);
            ByteArrayReadWriteTest_With_Index_Multi(opc.Session);

            //var node = "ns=3;i=1007";

            //var random = new Random((int)DateTime.Now.Ticks);
            //var vector = new ProsysVector
            //{
            //    X = random.NextDouble() * 100,
            //    Y = random.NextDouble() * 100,
            //    Z = random.NextDouble() * 100
            //};

            //var ret = opc.Session.Write(node, vector);
            //var _vector = opc.Session.Read<ProsysVector>(node);
            //Assert.Equal(vector.X, _vector?.X);
            //Assert.Equal(vector.Y, _vector?.Y);
            //Assert.Equal(vector.Z, _vector?.Z);

            opc.Disconnect();
        }

        [Fact]
        public async Task MainTestAsync()
        {
            EncodingFactory.Instance.Register(new Assembly[]
            { typeof(IEncodingFactory).Assembly,
                Assembly.GetEntryAssembly()
            });

            var opc = new OpcUaSession(_server);
            opc.Connect();

            await ByteArrayReadWriteTest_With_Index_Range_Async(opc.Session);
            await ByteArrayReadWriteTest_With_Index_One_Async(opc.Session);

            opc.Disconnect();
        }

        public void ByteArrayReadWriteTest_With_Range_Values(Session session)
        {
            Output.WriteLine($"------{MethodBase.GetCurrentMethod().Name}------");
            var node = "ns=4;s=GouIntArray";
            var _node = new NodeId("ns=4;s=GouIntArray");
            var random = new Random((int)DateTime.Now.Ticks);

            var count = random.Next(1, 5);
            var values = new short[count];
            int f = random.Next(0, 3);
            int s = random.Next(0, 5 - f);

            for (int i = 0; i < count; i++)
            {
                values[i] = (short)random.Next(short.MinValue, short.MaxValue);
            }

            var ret = session.Write(_node, values, f, s);
            values = values.Take(s).ToArray();
            Output.WriteLine($"Write {node}={string.Join(",", values)} {ret}");
            var _values = session.Read<short>(_node, f, s);
            Output.WriteLine($"Read {node}={string.Join(",", _values)} {values.SequenceEqual(_values)}");
            Assert.True(values.SequenceEqual(_values));
            Assert.Equal(values, _values);

            count = random.Next(1, 5);
            values = new short[count];
            f = random.Next(0, 3);
            s = random.Next(0, 5 - f);

            for (int i = 0; i < count; i++)
            {
                values[i] = (short)random.Next(short.MinValue, short.MaxValue);
            }

            ret = session.Write(node, values, f, s);
            values = values.Take(s).ToArray();
            Output.WriteLine($"Write {node}={string.Join(",", values)} {ret}");
            _values = session.Read<short>(node, f, s);
            Output.WriteLine($"Read {node}={string.Join(",", _values)} {values.SequenceEqual(_values)}");
            Assert.True(values.SequenceEqual(_values));
            Assert.Equal(values, _values);
        }

        public void ByteArrayReadWriteTest_With_Index_Range(Session session)
        {
            Output.WriteLine($"------{MethodBase.GetCurrentMethod().Name}------");
            var node = "ns=4;s=GouIntArray";
            var _node = new NodeId("ns=4;s=GouIntArray");
            var random = new Random((int)DateTime.Now.Ticks);

            int f = random.Next(0, 3);
            int s = random.Next(1, 3);
            var b = (short)random.Next(short.MinValue, short.MaxValue);
            var values = Enumerable.Repeat(b, s).ToArray();

            var ret = session.Write(_node, b, f, s);
            Output.WriteLine($"Write {node}={string.Join(",", values)} {ret}");
            var _values = session.Read<short>(_node, f, s);
            Output.WriteLine($"Read {node}={string.Join(",", _values)} {values.SequenceEqual(_values)}");
            Assert.True(values.SequenceEqual(_values));
            Assert.Equal(values, _values);

            f = random.Next(0, 3);
            s = random.Next(1, 3);
            b = (short)random.Next(short.MinValue, short.MaxValue);
            values = Enumerable.Repeat(b, s).ToArray();

            ret = session.Write(node, b, f, s);
            Output.WriteLine($"Write {node}={string.Join(",", values)} {ret}");
            _values = session.Read<short>(node, f, s);
            Output.WriteLine($"Read {node}={string.Join(",", _values)} {values.SequenceEqual(_values)}");
            Assert.True(values.SequenceEqual(_values));
            Assert.Equal(values, _values);
        }

        public void ByteArrayReadWriteTest_With_Index_One(Session session)
        {
            Output.WriteLine($"------{MethodBase.GetCurrentMethod().Name}------");
            var node = "ns=4;s=GouIntArray";
            var _node = new NodeId("ns=4;s=GouIntArray");
            var random = new Random((int)DateTime.Now.Ticks);

            var c = random.Next(1, 10);
            for (int i = 0; i < c; i++)
            {
                var b = (short)random.Next(short.MinValue, short.MaxValue);
                var index = random.Next(0, 5);
                var ret = session.Write(_node, b, index);
                Output.WriteLine($"Write {node}[{index}]={b} {ret}");
                var _b = session.Read<short>(_node, index, default);
                Output.WriteLine($"Read {node}=[{index}]={_b} {b == _b}");
                Assert.True(b == _b);
                Assert.Equal(b, _b);
            }

            c = random.Next(1, 10);
            for (int i = 0; i < c; i++)
            {
                var b = (short)random.Next(short.MinValue, short.MaxValue);
                var index = random.Next(0, 5);
                var ret = session.Write(node, b, index);
                Output.WriteLine($"Write {node}[{index}]={b} {ret}");
                var _b = session.Read<short>(node, index, default);
                Output.WriteLine($"Read {node}=[{index}]={_b} {b == _b}");
                Assert.True(b == _b);
                Assert.Equal(b, _b);
            }
        }

        public void ByteArrayReadWriteTest_With_Index_Multi(Session session)
        {
            Output.WriteLine($"------{MethodBase.GetCurrentMethod().Name}------");
            var node = "ns=4;s=GouIntArray";
            var _node = new NodeId("ns=4;s=GouIntArray");
            var random = new Random((int)DateTime.Now.Ticks);

            var count = random.Next(2, 5);
            short b = (short)random.Next(short.MinValue, short.MaxValue);
            var values = Enumerable.Repeat(b, count).ToArray();
            var indexs = new int[count];
            for (int i = 0; i < count; i++)
            {
                do
                {
                    var t = random.Next(0, 5);
                    if (!indexs.Contains(t))
                    {
                        indexs[i] = t;
                        break;
                    }
                } while (true);
            }

            var ret = session.Write(_node, b, indexs);
            Output.WriteLine($"Write {node}={string.Join(",", values)} {ret}");
            var _values = session.Read<short>(_node, indexs);
            Output.WriteLine($"Read {node}={string.Join(",", _values)} {values.SequenceEqual(_values)}");
            Assert.True(values.SequenceEqual(_values));
            Assert.Equal(values, _values);

            count = random.Next(2, 5);
            b = (short)random.Next(0, short.MaxValue);
            values = Enumerable.Repeat(b, count).ToArray();
            indexs = new int[count];
            for (int i = 0; i < count; i++)
            {
                do
                {
                    var t = random.Next(0, 5);
                    if (!indexs.Contains(t))
                    {
                        indexs[i] = t;
                        break;
                    }
                } while (true);
            }

            ret = session.Write(node, b, indexs);
            Output.WriteLine($"Write {node}={string.Join(",", values)} {ret}");
            _values = session.Read<short>(node, indexs);
            Output.WriteLine($"Read {node}={string.Join(",", _values)} {values.SequenceEqual(_values)}");
            Assert.True(values.SequenceEqual(_values));
            Assert.Equal(values, _values);
        }

        public async Task ByteArrayReadWriteTest_With_Index_Range_Async(Session session)
        {
            var node = "ns=4;s=GouIntArray";
            var _node = new NodeId("ns=4;s=GouIntArray");
            var random = new Random((int)DateTime.Now.Ticks);

            byte b = (byte)random.Next(0, byte.MaxValue);
            int s = (byte)random.Next(2, 5);
            var values = Enumerable.Repeat(b, s).ToArray();

            var ret = await session.WriteAsync(_node, b, 0, s);
            Output.WriteLine($"Write {node}={string.Join(",", values)} {ret}");
            var _values = await session.ReadAsync<byte>(_node, 0, s);
            Output.WriteLine($"Read {node}={string.Join(",", _values)} {values.SequenceEqual(_values)}");
            Assert.True(values.SequenceEqual(_values));
            Assert.Equal(values, _values);

            b = (byte)random.Next(0, byte.MaxValue);
            s = (byte)random.Next(2, 5);
            values = Enumerable.Repeat(b, s).ToArray();

            ret = await session.WriteAsync(node, b, 0, s);
            Output.WriteLine($"Write {node}={string.Join(",", values)} {ret}");
            _values = await session.ReadAsync<byte>(node, 0, s);
            Output.WriteLine($"Read {node}={string.Join(",", _values)} {values.SequenceEqual(_values)}");
            Assert.True(values.SequenceEqual(_values));
            Assert.Equal(values, _values);
        }

        public async Task ByteArrayReadWriteTest_With_Index_One_Async(Session session)
        {
            var node = "ns=4;s=GouIntArray";
            var _node = new NodeId("ns=4;s=GouIntArray");
            var random = new Random((int)DateTime.Now.Ticks);

            var c = random.Next(1, 10);
            for (int i = 0; i < c; i++)
            {
                var b = (byte)random.Next(0, byte.MaxValue);
                var index = random.Next(0, 5);
                var ret = await session.WriteAsync(_node, b, index);
                Output.WriteLine($"Write {node}[{index}]={b} {ret}");
                byte _b = await session.ReadAsync<byte>(_node, index, default);
                Output.WriteLine($"Read {node}=[{index}]={_b} {b == _b}");
                Assert.True(b == _b);
                Assert.Equal(b, _b);
            }

            c = random.Next(1, 10);
            for (int i = 0; i < c; i++)
            {
                var b = (byte)random.Next(0, byte.MaxValue);
                var index = random.Next(0, 5);
                var ret = await session.WriteAsync(node, b, index);
                Output.WriteLine($"Write {node}[{index}]={b} {ret}");
                byte _b = await session.ReadAsync<byte>(node, index, default);
                Output.WriteLine($"Read {node}=[{index}]={_b} {b == _b}");
                Assert.True(b == _b);
                Assert.Equal(b, _b);
            }
        }
    }
}
