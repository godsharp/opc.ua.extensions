
using System;

namespace GodSharp.Extensions.Opc.Ua.Types
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ComplexObjectGeneratorAttribute : Attribute
    {
        public ComplexObjectType ObjectType { get; set; }
        public EncodingMethodType MethodType { get; }

        public ComplexObjectGeneratorAttribute(
            ComplexObjectType objectType = ComplexObjectType.EncodeableObject,
            EncodingMethodType methodType = EncodingMethodType.Factory)
        {
            ObjectType = objectType;
            MethodType = methodType;
        }
    }
}
