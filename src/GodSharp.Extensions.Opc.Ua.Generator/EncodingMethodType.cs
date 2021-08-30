using System.ComponentModel;

namespace GodSharp.Extensions.Opc.Ua.Types
{
    [DefaultValue(Factory)]
    public enum EncodingMethodType
    {
        Factory,
        Extension
    }

    [DefaultValue(EncodeableObject)]
    public enum ComplexObjectType
    {
        EncodeableObject,
        SwitchField,
        OptionalField
    }
}
