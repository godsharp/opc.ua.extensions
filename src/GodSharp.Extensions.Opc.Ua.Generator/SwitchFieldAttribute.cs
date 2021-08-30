
using System;

namespace GodSharp.Extensions.Opc.Ua.Types
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class SwitchFieldAttribute : Attribute
    {
        public uint[] Values { get; set; }

        public SwitchFieldAttribute(params uint[] switchFieldValues)
        {
            Values = switchFieldValues;
        }
    }
}
