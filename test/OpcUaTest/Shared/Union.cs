using GodSharp.Extensions.Opc.Ua.Types;

namespace OpcUaTest
{
    [ComplexObjectGenerator(ComplexObjectType.SwitchField)]
    public partial class UaAnsiUnion
    {
        [SwitchField(1)]
        public int Int32;
        [SwitchField(2)]
        public string String;

        public UaAnsiUnion()
        {
            //TypeIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=3006";
            //BinaryEncodingIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=5003";
        }
    }
}