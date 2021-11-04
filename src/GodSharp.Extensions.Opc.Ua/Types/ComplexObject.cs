
using GodSharp.Extensions.Opc.Ua.Types.Encodings;
using Opc.Ua;

namespace GodSharp.Extensions.Opc.Ua.Types
{
    /// <summary>
    /// ComplexObject implement from <seealso cref="EncodeableObject"/>
    /// </summary>
    public abstract class ComplexObject : EncodeableObject
    {
        protected string _TypeIdNamespace;
        protected virtual string TypeIdNamespace 
        {
            get 
            {
                if(_TypeIdNamespace == null)
                {
                    SetNamespace();
                }
                return _TypeIdNamespace;
            }
            set
            {
                _TypeIdNamespace = value;
            }
        }

        protected virtual string BinaryEncodingIdNamespace { get; set; }
        protected virtual string XmlEncodingIdNamespace { get; set; }

        public override ExpandedNodeId XmlEncodingId
            => string.IsNullOrWhiteSpace(XmlEncodingIdNamespace)
                ? NodeId.Null
                : ExpandedNodeId.Parse(XmlEncodingIdNamespace);

        public override ExpandedNodeId TypeId
            => ExpandedNodeId.Parse(TypeIdNamespace);

        public override ExpandedNodeId BinaryEncodingId
            => ExpandedNodeId.Parse(BinaryEncodingIdNamespace);

        //protected ComplexObject()
        //{
        //    SetNamespace();
        //}

        private void SetNamespace()
        {
            var namespaces = EncodingFactory.Instance.GetTypeNamespace(GetType());
            if (namespaces == null) return;

            TypeIdNamespace = namespaces.TypeId;
            BinaryEncodingIdNamespace = namespaces.BinaryEncodingId;
            XmlEncodingIdNamespace = namespaces.XmlEncodingId;
        }
    }

    public abstract class SwitchFiledObject : ComplexObject
    {
        public uint SwitchField;

        protected SwitchFiledObject() : base()
        {
        }

        public override void Decode(IDecoder decoder)
        {
            base.Decode(decoder);

            SwitchField = decoder.ReadUInt32("SwitchField");
            switch (SwitchField)
            {
                case 0:
                    decoder.Read(ref SwitchField, "SwitchField");
                    break;
                default:
                    break;
            }
        }

        public override void Encode(IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.WriteUInt32("SwitchField", SwitchField);
            switch (SwitchField)
            {
                case 0:
                    encoder.Write(SwitchField, "SwitchField");
                    break;
                default:
                    break;
            }
        }
    }

    public abstract class OptionalFiledObject : ComplexObject
    {
        public uint EncodingMask;

        protected OptionalFiledObject() : base()
        {
        }

        public override void Decode(IDecoder decoder)
        {
            base.Decode(decoder);

            EncodingMask = decoder.ReadUInt32("EncodingMask");
            uint attribute = 0;
            if ((attribute & EncodingMask) != 0)
            {
                decoder.Read(ref EncodingMask, "SwitchField");
            }
        }

        public override void Encode(IEncoder encoder)
        {
            base.Encode(encoder);
            encoder.WriteUInt32("EncodingMask", EncodingMask);
            uint attribute = 0;
            if ((attribute& EncodingMask)!=0)
            {
                encoder.Write(EncodingMask, "SwitchField");
            }
        }
    }
}
