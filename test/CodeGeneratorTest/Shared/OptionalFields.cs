using GodSharp.Extensions.Opc.Ua.Types;

namespace CodeGeneratorTest
{

    [ComplexObjectGenerator(ComplexObjectType.OptionalField)]
    public partial class OptionalFields : ComplexObject
    {
        public int MandatoryInt32;
        [OptionalField(0b0001)]
        public int? OptionalInt32;
        public int NoOfMandatoryStringArray => MandatoryStringArray?.Length ?? 0;
        public string[] MandatoryStringArray;
        public int NoOfOptionalStringArray => OptionalStringArray?.Length ?? 0;
        [OptionalField(0b0010)]
        public string[] OptionalStringArray;

        public OptionalFields()
        {
            TypeIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=3007";
            BinaryEncodingIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=5005";
        }
    }

    [ComplexObjectGenerator(ComplexObjectType.OptionalField)]
    public partial class AccessRights : ComplexObject
    {
        //Opc.Ua.EnumField
        //public int Value;
        //public int ValidBits;
        [OptionalField(0b0001)]
        public bool? Read;
        [OptionalField(0b0010)]
        public bool? Write;
        [OptionalField(0b0100)]
        public bool? Execute;

        public AccessRights()
        {
            TypeIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=3008";
            BinaryEncodingIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=5007";
        }
    }
}