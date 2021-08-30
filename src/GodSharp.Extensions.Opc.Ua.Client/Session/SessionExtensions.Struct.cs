using GodSharp.Extensions.Opc.Ua.Utilities;
using Opc.Ua;
using Opc.Ua.Client;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable CheckNamespace

namespace GodSharp.Extensions.Opc.Ua.Client
{
    public static partial class SessionExtensions
    {
        /// <summary>
        /// Read a struct value
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the <see cref="NodeId"/>.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <typeparam name="T">the value type.</typeparam>
        /// <returns>an set value of <see cref="T"/>.</returns>
        public static T ReadStruct<T>(this Session session, NodeId node, T defaultValue = default) where T : struct
            => session.Read(node)
                ?.Value is ExtensionObject {Body: byte[] {Length: > 0} buffer}
                ? StructConverter.GetStruct<T>(buffer)
                : defaultValue;

        /// <summary>
        /// Read a struct value
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the <see cref="string"/>.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <typeparam name="T">the value type.</typeparam>
        /// <returns>an set value of <see cref="T"/>.</returns>
        public static T ReadStruct<T>(this Session session, string node, T defaultValue = default) where T : struct
            => session.ReadStruct(new NodeId(node), defaultValue);

        /// <summary>
        /// Write a struct value
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node id.</param>
        /// <param name="value">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool WriteStruct<T>(this Session session, NodeId node, T value) where T : struct
            => session.Write(node, new ExtensionObject(node, StructConverter.GetBytes(value)));

        /// <summary>
        /// Write a struct value
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node id.</param>
        /// <param name="value">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool WriteStruct<T>(this Session session, string node, T value) where T : struct
            => session.WriteStruct(new NodeId(node), value);
    }
}