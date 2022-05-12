using CommonTest;

using GodSharp.Extensions.Opc.Ua.Client;

using Xunit;
using Xunit.Abstractions;

namespace OpcUaTest
{
    public class KepServerOpcUaUnitTest : UnitTestBase
    {
        private readonly string _server = "opc.tcp://127.0.0.1:49320";

        public KepServerOpcUaUnitTest(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public void MainTest()
        {
            var opc = new OpcUaSession(_server);
            opc.Connect();
            var tree = opc.Session.BrowseTree();
            //var tree = client.Session.BrowseTree(new NodeId("ns=2;s=模拟器示例.函数._Hints"));
            Browse(tree, Output);

            static void Browse(in ReferenceBrowseDescription[] refs, ITestOutputHelper output, int level = -1)
            {
                level++;
                foreach (var description in refs)
                {
                    output.WriteLine("{0}+{1}, {2},{3}",
                        new string('\t', level),
                        //Formatter.FormatAttributeValue(attribute.ValueId.AttributeId, attribute.Value)}
                        //description.Node.BrowseName,
                        description.GetFormatText(),
                        description.Node.NodeClass,
                        description.Node.NodeId
                    );
                    if (description.Children != null)
                    {
                        Browse(description.Children, output, level);
                    }
                }
            }

            opc.Disconnect();
        }
    }
}