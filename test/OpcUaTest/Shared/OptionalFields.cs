using GodSharp.Extensions.Opc.Ua.Types;
using GodSharp.Extensions.Opc.Ua.Types.Encodings;
using static GodSharp.Extensions.Opc.Ua.Types.Encodings.EncodingFactory;

using Opc.Ua;
using System.Collections.Generic;

namespace OpcUaTest
{
    [ComplexObjectGenerator(ComplexObjectType.OptionalField, EncodingMethodType.Extension)]
    public partial class OptionalFields : ComplexObject
    {
        public int MandatoryInt32;
        [OptionalField(0x01)]
        public int OptionalInt32;
        public int NoOfMandatoryStringArray => MandatoryStringArray?.Length ?? 0;
        public string[] MandatoryStringArray;
        public int NoOfOptionalStringArray => OptionalStringArray?.Length ?? 0;
        [OptionalField(0x02)]
        public string[] OptionalStringArray;

        public OptionalFields()
        {
            TypeIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=3007";
            BinaryEncodingIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=5005";
        }
    }

    //[ComplexObjectGenerator(ComplexObjectType.OptionalField)]
    //public partial class AccessRights : ComplexObject
    //{
    //    //public uint EncodingMask;

    //    //Opc.Ua.EnumField
    //    public byte[] Value;
    //    public byte[] ValidBits;
    //    [OptionalField(0b0001)]
    //    public bool Read;
    //    [OptionalField(0b0010)]
    //    public bool Write;
    //    [OptionalField(0b0100)]
    //    public bool Execute;

    //    //public EnumField Read;
    //    //public EnumField Write;
    //    //public EnumField Execute;

    //    public AccessRights() : base()
    //    {
    //        //TypeIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=3008";
    //        //BinaryEncodingIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=5007";

    //        //Fields = new EnumFieldCollection();
    //        //Fields.Add(new EnumField() { Name = "Read", DisplayName = "", Description = "" });
    //        //Fields.Add(new EnumField() { Name = "Write", DisplayName = "", Description = "" });
    //        //Fields.Add(new EnumField() { Name = "Execute", DisplayName = "", Description = "" });
    //    }

    //    public override void Encode(IEncoder encoder)
    //    {
    //        base.Encode(encoder);

    //        //encoder.WriteInt64("Value", Value);
    //        //encoder.WriteLocalizedText("DisplayName", DisplayName);
    //        //encoder.WriteLocalizedText("Description", Description);

    //        ////encoder.WriteEncodeableArray("Fields", Fields.ToArray(), typeof(EnumField));

    //        //encoder.WriteUInt32("EncodingMask", EncodingMask);
    //        //if ((0b0001 & EncodingMask) != 0)
    //        //{
    //        //    Encoding.Write(encoder, Read, nameof(Read));
    //        //}
    //        //if ((0b0010 & EncodingMask) != 0)
    //        //{
    //        //    Encoding.Write(encoder, Write, nameof(Write));
    //        //}
    //        //if ((0b0100 & EncodingMask) != 0)
    //        //{
    //        //    Encoding.Write(encoder, Execute, nameof(Execute));
    //        //}
    //    }

    //    public override void Decode(IDecoder decoder)
    //    {
    //        base.Decode(decoder);

    //        //var array = (EnumFieldCollection)decoder.ReadEncodeableArray("Fields", typeof(EnumField));
    //        Value = decoder.ReadByteString("Value");
    //        ValidBits = decoder.ReadByteString("ValidBits");

    //        //EncodingMask = decoder.ReadUInt32("EncodingMask");
    //        //if ((0b0001 & EncodingMask) != 0)
    //        //{
    //        //    Encoding.Read(decoder, ref Read, nameof(Read));
    //        //}
    //        //if ((0b0010 & EncodingMask) != 0)
    //        //{
    //        //    Encoding.Read(decoder, ref Write, nameof(Write));
    //        //}
    //        //if ((0b0100 & EncodingMask) != 0)
    //        //{
    //        //    Encoding.Read(decoder, ref Execute, nameof(Execute));
    //        //}
    //    }
    //}
}