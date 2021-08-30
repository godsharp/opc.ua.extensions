using Opc.Ua;

using System;
using System.Linq;
using System.Xml;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

// ReSharper disable once CheckNamespace
namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public static class EncodingExtensions
    {
        //private static Type NullableType = typeof(Nullable<>);
        private static Type EncodeableType = typeof(IEncodeable);

        public static void Write<T>(this IEncoder encoder, T value, string name)
        {
            //var type = typeof(T);
            //if (type.IsGenericType && type.GetGenericTypeDefinition() == NullableType)
            //{
            //    type = type.GetGenericArguments()?.FirstOrDefault();
            //}

            switch (typeof(T).Name)
            {
                case ObjectTypes.Boolean:
                    {
                        if (value is not bool v) v = default;
                        encoder.WriteBoolean(name, v);
                    }
                    break;
                case ObjectTypes.SByte:
                    {
                        if (value is not sbyte v) v = default;
                        encoder.WriteSByte(name, v);
                    }
                    break;
                case ObjectTypes.Int16:
                    {
                        if (value is not short v) v = default;
                        encoder.WriteInt16(name, v);
                    }
                    break;
                case ObjectTypes.Int32:
                    {
                        if (value is not int v) v = default;
                        encoder.WriteInt32(name, v);
                    }
                    break;
                case ObjectTypes.Int64:
                    {
                        if (value is not long v) v = default;
                        encoder.WriteInt64(name, v);
                    }
                    break;
                case ObjectTypes.Byte:
                    {
                        if (value is not byte v) v = default;
                        encoder.WriteByte(name, v);
                    }
                    break;
                case ObjectTypes.UInt16:
                    {
                        if (value is not ushort v) v = default;
                        encoder.WriteUInt16(name, v);
                    }
                    break;
                case ObjectTypes.UInt32:
                    {
                        if (value is not uint v) v = default;
                        encoder.WriteUInt32(name, v);
                    }
                    break;
                case ObjectTypes.UInt64:
                    {
                        if (value is not ulong v) v = default;
                        encoder.WriteUInt64(name, v);
                    }
                    break;
                case ObjectTypes.Float:
                    {
                        if (value is not float v) v = default;
                        encoder.WriteFloat(name, v);
                    }
                    break;
                case ObjectTypes.Double:
                    {
                        if (value is not double v) v = default;
                        encoder.WriteDouble(name, v);
                    }
                    break;
                case ObjectTypes.String:
                    {
                        if (value is not string v) v = default;
                        encoder.WriteString(name, v);
                    }
                    break;
                case ObjectTypes.DateTime:
                    {
                        if (value is not DateTime v) v = default;
                        encoder.WriteDateTime(name, v);
                    }
                    break;
                case ObjectTypes.Guid:
                    {
                        if (value is not Guid v) v = default;
                        encoder.WriteGuid(name, v);
                    }
                    break;
                case ObjectTypes.Uuid:
                    {
                        if (value is not Uuid v) v = default;
                        encoder.WriteGuid(name, v);
                    }
                    break;
                case ObjectTypes.XmlElement:
                    {
                        if (value is not XmlElement v) v = default;
                        encoder.WriteXmlElement(name, v);
                    }
                    break;
                case ObjectTypes.Variant:
                    {
                        if (value is not Variant v) v = default;
                        encoder.WriteVariant(name, v);
                    }
                    break;
                case ObjectTypes.DataValue:
                    {
                        if (value is not DataValue v) v = default;
                        encoder.WriteDataValue(name, v);
                    }
                    break;
                case ObjectTypes.NodeId:
                    {
                        if (value is not NodeId v) v = default;
                        encoder.WriteNodeId(name, v);
                    }
                    break;
                case ObjectTypes.ExpandedNodeId:
                    {
                        if (value is not ExpandedNodeId v) v = default;
                        encoder.WriteExpandedNodeId(name, v);
                    }
                    break;
                case ObjectTypes.ExtensionObject:
                    {
                        if (value is not ExtensionObject v) v = default;
                        encoder.WriteExtensionObject(name, v);
                    }
                    break;
                case ObjectTypes.QualifiedName:
                    {
                        if (value is not QualifiedName v) v = default;
                        encoder.WriteQualifiedName(name, v);
                    }
                    break;
                case ObjectTypes.LocalizedText:
                    {
                        if (value is not LocalizedText v) v = default;
                        encoder.WriteLocalizedText(name, v);
                    }
                    break;
                case ObjectTypes.StatusCode:
                    {
                        if (value is not StatusCode v) v = default;
                        encoder.WriteStatusCode(name, v);
                    }
                    break;
                case ObjectTypes.DiagnosticInfo:
                    {
                        if (value is not DiagnosticInfo v) v = default;
                        encoder.WriteDiagnosticInfo(name, v);
                    }
                    break;
                case ObjectTypes.BooleanArray:
                    {
                        if (value is not bool[] v) v = default;
                        encoder.WriteBooleanArray(name, v);
                    }
                    break;
                case ObjectTypes.SByteArray:
                    {
                        if (value is not sbyte[] v) v = default;
                        encoder.WriteSByteArray(name, v);
                    }
                    break;
                case ObjectTypes.Int16Array:
                    {
                        if (value is not short[] v) v = default;
                        encoder.WriteInt16Array(name, v);
                    }
                    break;
                case ObjectTypes.Int32Array:
                    {
                        if (value is not int[] v) v = default;
                        encoder.WriteInt32Array(name, v);
                    }
                    break;
                case ObjectTypes.Int64Array:
                    {
                        if (value is not long[] v) v = default;
                        encoder.WriteInt64Array(name, v);
                    }
                    break;
                case ObjectTypes.ByteArray:
                    {
                        if (value is not byte[] v) v = default;
                        encoder.WriteByteArray(name, v);
                    }
                    break;
                case ObjectTypes.UInt16Array:
                    {
                        if (value is not ushort[] v) v = default;
                        encoder.WriteUInt16Array(name, v);
                    }
                    break;
                case ObjectTypes.UInt32Array:
                    {
                        if (value is not uint[] v) v = default;
                        encoder.WriteUInt32Array(name, v);
                    }
                    break;
                case ObjectTypes.UInt64Array:
                    {
                        if (value is not ulong[] v) v = default;
                        encoder.WriteUInt64Array(name, v);
                    }
                    break;
                case ObjectTypes.FloatArray:
                    {
                        if (value is not float[] v) v = default;
                        encoder.WriteFloatArray(name, v);
                    }
                    break;
                case ObjectTypes.DoubleArray:
                    {
                        if (value is not double[] v) v = default;
                        encoder.WriteDoubleArray(name, v);
                    }
                    break;
                case ObjectTypes.StringArray:
                    {
                        if (value is not string[] v) v = default;
                        encoder.WriteStringArray(name, v);
                    }
                    break;
                case ObjectTypes.DateTimeArray:
                    {
                        if (value is not DateTime[] v) v = default;
                        encoder.WriteDateTimeArray(name, v);
                    }
                    break;
                case ObjectTypes.GuidArray:
                    {
                        if (value is not Guid[] v) v = default;
                        encoder.WriteGuidArray(name, v);
                    }
                    break;
                case ObjectTypes.UuidArray:
                    {
                        if (value is not Uuid[] v) v = default;
                        encoder.WriteGuidArray(name, v);
                    }
                    break;
                case ObjectTypes.XmlElementArray:
                    {
                        if (value is not XmlElement[] v) v = default;
                        encoder.WriteXmlElementArray(name, v);
                    }
                    break;
                case ObjectTypes.VariantArray:
                    {
                        if (value is not Variant[] v) v = default;
                        encoder.WriteVariantArray(name, v);
                    }
                    break;
                case ObjectTypes.DataValueArray:
                    {
                        if (value is not DataValue[] v) v = default;
                        encoder.WriteDataValueArray(name, v);
                    }
                    break;
                case ObjectTypes.NodeIdArray:
                    {
                        if (value is not NodeId[] v) v = default;
                        encoder.WriteNodeIdArray(name, v);
                    }
                    break;
                case ObjectTypes.ExpandedNodeIdArray:
                    {
                        if (value is not ExpandedNodeId[] v) v = default;
                        encoder.WriteExpandedNodeIdArray(name, v);
                    }
                    break;
                case ObjectTypes.ExtensionObjectArray:
                    {
                        if (value is not ExtensionObject[] v) v = default;
                        encoder.WriteExtensionObjectArray(name, v);
                    }
                    break;
                case ObjectTypes.QualifiedNameArray:
                    {
                        if (value is not QualifiedName[] v) v = default;
                        encoder.WriteQualifiedNameArray(name, v);
                    }
                    break;
                case ObjectTypes.LocalizedTextArray:
                    {
                        if (value is not LocalizedText[] v) v = default;
                        encoder.WriteLocalizedTextArray(name, v);
                    }
                    break;
                case ObjectTypes.StatusCodeArray:
                    {
                        if (value is not StatusCode[] v) v = default;
                        encoder.WriteStatusCodeArray(name, v);
                    }
                    break;
                case ObjectTypes.DiagnosticInfoArray:
                    {
                        if (value is not DiagnosticInfo[] v) v = default;
                        encoder.WriteDiagnosticInfoArray(name, v);
                    }
                    break;
                default:
                    ExtensionWriteCallback(encoder, ref value, name);
                    break;
            }
        }

        public static void Read<T>(this IDecoder decoder, ref T value, string name)
        {
            //var type = typeof(T);
            //if (type.IsGenericType && type.GetGenericTypeDefinition() == NullableType)
            //{
            //    type = type.GetGenericArguments()?.FirstOrDefault();
            //}

            object result = typeof(T).Name switch
            {
                ObjectTypes.Boolean => decoder.ReadBoolean(name),
                ObjectTypes.SByte => decoder.ReadSByte(name),
                ObjectTypes.Int16 => decoder.ReadInt16(name),
                ObjectTypes.Int32 => decoder.ReadInt32(name),
                ObjectTypes.Int64 => decoder.ReadInt64(name),
                ObjectTypes.Byte => decoder.ReadByte(name),
                ObjectTypes.UInt16 => decoder.ReadUInt16(name),
                ObjectTypes.UInt32 => decoder.ReadUInt32(name),
                ObjectTypes.UInt64 => decoder.ReadUInt64(name),
                ObjectTypes.Float => decoder.ReadFloat(name),
                ObjectTypes.Double => decoder.ReadDouble(name),
                ObjectTypes.String => decoder.ReadString(name),
                ObjectTypes.DateTime => decoder.ReadDateTime(name),
                ObjectTypes.Guid => decoder.ReadGuid(name),
                ObjectTypes.Uuid => decoder.ReadGuid(name),
                ObjectTypes.XmlElement => decoder.ReadXmlElement(name),
                ObjectTypes.Variant => decoder.ReadVariant(name),
                ObjectTypes.DataValue => decoder.ReadDataValue(name),
                ObjectTypes.NodeId => decoder.ReadNodeId(name),
                ObjectTypes.ExpandedNodeId => decoder.ReadExpandedNodeId(name),
                ObjectTypes.ExtensionObject => decoder.ReadExtensionObject(name),
                ObjectTypes.QualifiedName => decoder.ReadQualifiedName(name),
                ObjectTypes.LocalizedText => decoder.ReadLocalizedText(name),
                ObjectTypes.StatusCode => decoder.ReadStatusCode(name),
                ObjectTypes.DiagnosticInfo => decoder.ReadDiagnosticInfo(name),
                ObjectTypes.BooleanArray => decoder.ReadBooleanArray(name)?.ToArray(),
                ObjectTypes.SByteArray => decoder.ReadSByteArray(name)?.ToArray(),
                ObjectTypes.Int16Array => decoder.ReadInt16Array(name)?.ToArray(),
                ObjectTypes.Int32Array => decoder.ReadInt32Array(name)?.ToArray(),
                ObjectTypes.Int64Array => decoder.ReadInt64Array(name)?.ToArray(),
                ObjectTypes.ByteArray => decoder.ReadByteArray(name)?.ToArray(),
                ObjectTypes.UInt16Array => decoder.ReadUInt16Array(name)?.ToArray(),
                ObjectTypes.UInt32Array => decoder.ReadUInt32Array(name)?.ToArray(),
                ObjectTypes.UInt64Array => decoder.ReadUInt64Array(name)?.ToArray(),
                ObjectTypes.FloatArray => decoder.ReadFloatArray(name)?.ToArray(),
                ObjectTypes.DoubleArray => decoder.ReadDoubleArray(name)?.ToArray(),
                ObjectTypes.StringArray => decoder.ReadStringArray(name)?.ToArray(),
                ObjectTypes.DateTimeArray => decoder.ReadDateTimeArray(name)?.ToArray(),
                ObjectTypes.GuidArray => decoder.ReadGuidArray(name)?.Select(x => (Guid)x).ToArray(),
                ObjectTypes.UuidArray => decoder.ReadGuidArray(name)?.ToArray(),
                ObjectTypes.XmlElementArray => decoder.ReadXmlElementArray(name)?.ToArray(),
                ObjectTypes.VariantArray => decoder.ReadVariantArray(name)?.ToArray(),
                ObjectTypes.DataValueArray => decoder.ReadDataValueArray(name)?.ToArray(),
                ObjectTypes.NodeIdArray => decoder.ReadNodeIdArray(name)?.ToArray(),
                ObjectTypes.ExpandedNodeIdArray => decoder.ReadExpandedNodeIdArray(name)?.ToArray(),
                ObjectTypes.ExtensionObjectArray => decoder.ReadExtensionObjectArray(name)?.ToArray(),
                ObjectTypes.QualifiedNameArray => decoder.ReadQualifiedNameArray(name)?.ToArray(),
                ObjectTypes.LocalizedTextArray => decoder.ReadLocalizedTextArray(name)?.ToArray(),
                ObjectTypes.StatusCodeArray => decoder.ReadStatusCodeArray(name)?.ToArray(),
                ObjectTypes.DiagnosticInfoArray => decoder.ReadDiagnosticInfoArray(name)?.ToArray(),
                _ => ExtensionReadCallback<T>(decoder, name)
            };

            if (result is not T t) return;
            value = t;
        }

        private static object ExtensionReadCallback<T>(IDecoder decoder, string name)
        {
            var type = typeof(T);

            if (type.IsArray)
            {
                var etype = type.GetElementType();
                if (etype == null) return null;

                // Enumerated
                if (etype.IsEnum)
                {
                    return decoder.ReadEnumeratedArray(name, etype);
                }

                // IEncodeable
                if (etype.IsSubclassOf(EncodeableType))
                {
                    return decoder.ReadEncodeableArray(name, etype);
                }
            }
            else
            {
                // Enumerated
                if (type.IsEnum)
                {
                    return decoder.ReadEnumerated(name, type);
                }

                // IEncodeable
                if (type.IsSubclassOf(EncodeableType))
                {
                    return decoder.ReadEncodeable(name, type);
                }
            }

            return null;
        }

        private static void ExtensionWriteCallback<T>(IEncoder encoder,ref T value, string name)
        {
            var type = typeof(T);

            if (type.IsArray)
            {
                var etype = type.GetElementType();
                if (etype == null) return;

                // Enumerated
                if (etype.IsEnum && value is Array ae)
                {
                    encoder.WriteEnumeratedArray(name, ae, etype);
                    return;
                }

                // IEncodeable
                if (etype.IsSubclassOf(EncodeableType))
                {
                    if (value is IEncodeable[] aencodeable) encoder.WriteEncodeableArray(name, aencodeable, etype);
                    return;
                }
            }
            else
            {
                // Enumerated
                if (type.IsEnum)
                {
                    if (value is Enum e) encoder.WriteEnumerated(name, e);
                    return;
                }

                // IEncodeable
                if (type.IsSubclassOf(EncodeableType))
                {
                    if (value is IEncodeable encodeable) encoder.WriteEncodeable(name, encodeable, type);
                    return;
                }
            }
        }
    }
}
