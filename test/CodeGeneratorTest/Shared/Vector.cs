using GodSharp.Extensions.Opc.Ua.Types;

namespace CodeGeneratorSourceGeneratorTest
{
    [ComplexObjectGenerator]
    public partial class UaAnsiCServerVector : ComplexObject
    {
        public double X;
        public double Y;
        public double Z;

        public UaAnsiCServerVector()
        {
            TypeIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=3002";
            BinaryEncodingIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=5054";
        }
    }

    [ComplexObjectGenerator]
    public partial class ProsysVector : ComplexObject
    {
        public double X;
        public double Y;
        public double Z;

        public ProsysVector()
        {
            TypeIdNamespace = "nsu=http://opcfoundation.org/UA/;i=18808";
            BinaryEncodingIdNamespace = "nsu=http://opcfoundation.org/UA/;i=18817";
        }
    }
}