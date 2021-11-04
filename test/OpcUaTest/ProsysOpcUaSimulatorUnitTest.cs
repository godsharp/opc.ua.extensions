using GodSharp.Extensions.Opc.Ua.Client;
using Opc.Ua;
using System.Reflection;
using System;
using CommonTest;
using Xunit;
using GodSharp.Extensions.Opc.Ua.Types.Encodings;
using GodSharp.Extensions.Opc.Ua.Types;

namespace OpcUaTest
{
    public partial class ProsysOpcUaSimulatorUnitTest
    {
        private readonly string _server = "opc.tcp://GodSharp:53530/OPCUA/SimulationServer";

        [Fact]
        public void MainTest()
        {
            EncodingFactory.Instance.Register(new Assembly[]
            { typeof(IEncodingFactory).Assembly,
                Assembly.GetEntryAssembly()
            });

            var opc = new OpcUaSession(_server);
            opc.Connect();

            var node = "ns=3;i=1007";

            var random = new Random((int)DateTime.Now.Ticks);
            var vector = new ProsysVector
            {
                X = random.NextDouble() * 100,
                Y = random.NextDouble() * 100,
                Z = random.NextDouble() * 100
            };

            var ret = opc.Session.Write(node, vector);
            var _vector = opc.Session.Read<ProsysVector>(node);
            Assert.Equal(vector.X, _vector?.X);
            Assert.Equal(vector.Y, _vector?.Y);
            Assert.Equal(vector.Z, _vector?.Z);

            opc.Disconnect();
        }
    }
}
