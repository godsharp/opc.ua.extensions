using Opc.Ua;

using System;
using System.Reflection;

namespace GodSharp.Extensions.Opc.Ua
{
    public static class DataValueExtension
    {
        public static T ValueOf<T>(this DataValue dv, T defaultValue = default)
        {
            if (StatusCode.IsNotGood(dv.StatusCode))
            {
                return defaultValue;
            }
            var type = typeof(T);

            if (type.IsEnum)
            {
                return (T)Enum.ToObject(type, dv.Value);
            }

            if (type.IsInstanceOfType(dv.Value))
            {
                if(type.IsArray)
                {
                    var etype = type.GetElementType();

                    if(etype.IsEnum && dv.Value is Array arr)
                    {
                        object array1 = new();
                        array1 = type
                            .InvokeMember(
                                "Set",
                                BindingFlags.CreateInstance,
                                null,
                                array1,
                                new object[] { arr.Length }
                            );
                        for (int i = 0; i < arr.Length; i++)
                        {
                            type
                                .GetMethod("SetValue", new Type[2] { typeof(object), typeof(int) })
                                .Invoke(array1, new object[] { Enum.ToObject(etype, arr.GetValue(i)), i });
                        }

                        return (T)array1;
                    }
                }

                return (T)dv.Value;
            }

            if (dv.Value is ExtensionObject extensionObject && type.IsInstanceOfType(extensionObject.Body))
            {
                return (T)extensionObject.Body;
            }

            return defaultValue;
        }
    }
}
