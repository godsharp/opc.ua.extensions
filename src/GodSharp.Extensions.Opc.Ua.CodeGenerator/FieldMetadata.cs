using GodSharp.Extensions.Opc.Ua.Types;

using System.ComponentModel;

namespace GodSharp.Extensions.Opc.Ua.CodeGenerator
{
    public class FieldMetadata
    {
        public string Name { get; set; }
        public FieldType Type { get; set; }
        public SwitchFieldData SwitchField { get; set; }
        public OptionalFieldData OptionalField { get; set; }

        public FieldMetadata()
        {
        }

        public FieldMetadata(string name, FieldType type = FieldType.Generic)
        {
            Name = name;
            Type = type;
        }

        public FieldMetadata(string name, SwitchFieldData attribute) : this(name, FieldType.Switch)
        {
            SwitchField = attribute;
        }

        public FieldMetadata(string name, OptionalFieldData attribute) : this(name, FieldType.Optional)
        {
            OptionalField = attribute;
        }

        public class OptionalFieldData
        {
            public string Mask { get; set; }

            public OptionalFieldData(string mask)
            {
                Mask = mask;
            }
        }

        public class SwitchFieldData
        {
            public string[] Values { get; set; }

            public SwitchFieldData(params string[] switchFieldValues)
            {
                Values = switchFieldValues;
            }
        }
    }

    [DefaultValue(Generic)]
    public enum FieldType
    {
        Generic,
        Switch,
        Optional
    }
}
