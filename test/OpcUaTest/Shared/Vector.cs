using GodSharp.Extensions.Opc.Ua.Types;

namespace OpcUaTest
{
    [ComplexObjectGenerator]
    public partial class UaOrgVector : ComplexObject
    {
        public double X;
        public double Y;
        public double Z;

        public UaOrgVector()
        {
            TypeIdNamespace = "nsu=http://opcfoundation.org/UA/;i=18808";
            BinaryEncodingIdNamespace = "nsu=http://opcfoundation.org/UA/;i=18817";
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

    [ComplexObjectGenerator(ComplexObjectType.EncodeableObject, EncodingMethodType.Extension)]
    public partial class UaAnsiVector : ComplexObject
    {
        public double X;
        public double Y;
        public double Z;

        public UaAnsiVector()
        {
            TypeIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=3002";
            BinaryEncodingIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=5054";
        }
    }
}