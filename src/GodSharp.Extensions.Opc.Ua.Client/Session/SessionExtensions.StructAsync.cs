using System.Threading.Tasks;
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
        /// Read a struct value by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the <see cref="NodeId"/>.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <typeparam name="T">the value type.</typeparam>
        /// <returns>an set value of <see cref="T"/>.</returns>
        public static async Task<T> ReadStructAsync<T>(this Session session, NodeId node, T defaultValue = default) where T : struct
            => (await session.ReadAsync(node))
                ?.Value is ExtensionObject {Body: byte[] {Length: > 0} buffer}
                    ? StructConverter.GetStruct<T>(buffer)
                    : defaultValue;

        /// <summary>
        /// Read a struct value by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the <see cref="string"/>.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <typeparam name="T">the value type.</typeparam>
        /// <returns>an set value of <see cref="T"/>.</returns>
        public static async Task<T> ReadStructAsync<T>(this Session session, string node, T defaultValue = default) where T : struct
            => await session.ReadStructAsync(new NodeId(node), defaultValue);
        
        /// <summary>
        /// Write a struct value by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node id.</param>
        /// <param name="value">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static async Task<bool> WriteStructAsync<T>(this Session session, NodeId node, T value) where T : struct
            => await session.WriteAsync(node, new ExtensionObject(node, StructConverter.GetBytes(value)));
        
        /// <summary>
        /// Write a struct value by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node id.</param>
        /// <param name="value">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static async Task<bool> WriteStructAsync<T>(this Session session, string node, T value) where T : struct
            => await session.WriteStructAsync(new NodeId(node), value);
    }
}