
using System;

namespace GodSharp.Extensions.Opc.Ua.Types
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class OptionalFieldAttribute : Attribute
    {
        public uint Mask { get; set; }

        public OptionalFieldAttribute(uint mask)
        {
            Mask = mask;
        }
    }
}
