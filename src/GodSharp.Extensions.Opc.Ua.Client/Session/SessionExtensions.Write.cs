using System;
using System.Linq;

using Opc.Ua;
using Opc.Ua.Client;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedType.Global
// ReSharper disable CheckNamespace

namespace GodSharp.Extensions.Opc.Ua.Client
{
    public static partial class SessionExtensions
    {
        private static Type EnumType = typeof(Enum);
        
        /// <summary>
        /// Write values with specialized node ids.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write(this Session session, (NodeId node, object value)[] values)
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));

            var nodesToWrite = new WriteValueCollection(
                values.Select(x =>
                    new WriteValue
                    {
                        NodeId = x.node,
                        AttributeId = Attributes.Value,
                        Value =
                        {
                            Value = x.value, 
                            StatusCode = StatusCodes.Good, 
                            SourceTimestamp = DateTime.MinValue, 
                            ServerTimestamp = DateTime.MinValue
                        }
                    }
                )
            );

            session.Write(null, nodesToWrite, out var results, out var diagnosticInfos);
            ClientBase.ValidateResponse(results, nodesToWrite);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToWrite);

            return !results.Any(x=>StatusCode.IsBad(x.Code));
        }

        /// <summary>
        /// Write values with specialized node ids.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write(this Session session, (string nodeid, object value)[] values)
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.Write(values.Select(x => (new NodeId(x.nodeid), x.value)).ToArray());
        }

        /// <summary>
        /// Write value with specialized node id.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node id.</param>
        /// <param name="value">the value to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write(this Session session, NodeId node, object value)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            return session.Write(new (NodeId, object)[] { (node, value) });
        }

        /// <summary>
        /// Write value with specialized node id.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="value">the value to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write(this Session session, string nodeId, object value)
        {
            if (string.IsNullOrWhiteSpace(nodeId)) throw new ArgumentNullException(nameof(nodeId));
            return session.Write(new (NodeId, object)[] { (new NodeId(nodeId), value) });
        }

        /// <summary>
        /// Write enum array values with specialized node.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node.</param>
        /// <param name="values">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, NodeId node, T[] values) where T : Enum
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.Write(node, values.Select(x => Convert.ToInt32(x)).ToArray());
        }

        /// <summary>
        /// Write enum array values with specialized node id.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="values">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, string nodeId, T[] values) where T : Enum
        {
            if (string.IsNullOrWhiteSpace(nodeId)) throw new ArgumentNullException(nameof(nodeId));
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.Write(nodeId, values.Select(x => Convert.ToInt32(x)).ToArray());
        }

        /// <summary>
        /// Write enum value with specialized node id.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node.</param>
        /// <param name="value">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, NodeId node, T value) where T : Enum
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            return session.Write(node, Convert.ToInt32(value));
        }

        /// <summary>
        /// Write enum value with specialized node id.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="value">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, string nodeId, T value) where T : Enum
        {
            if (string.IsNullOrWhiteSpace(nodeId)) throw new ArgumentNullException(nameof(nodeId));
            return session.Write(nodeId, Convert.ToInt32(value));
        }

        /// <summary>
        /// Write array values with specialized array node ids and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        private static bool Write<T>(this Session session, (NodeId node, T[] value, string range)[] values)
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            var isEnum = typeof(T).IsSubclassOf(EnumType);

            var nodesToWrite = new WriteValueCollection(
                values.Select(x => new WriteValue
                {
                    NodeId = x.node,
                    AttributeId = Attributes.Value,
                    IndexRange = x.range,
                    Value =
                    {
                        Value =(object)(isEnum
                            ? x.value.Select(s=>Convert.ToInt32(s)).ToArray()
                            : x.value
                        ),
                        StatusCode = StatusCodes.Good,
                        SourceTimestamp = DateTime.MinValue,
                        ServerTimestamp = DateTime.MinValue
                    }
                })
            );

            session.Write(null, nodesToWrite, out var results, out var diagnosticInfos);
            ClientBase.ValidateResponse(results, nodesToWrite);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToWrite);

            //if (results.Any(x => x == StatusCodes.BadWriteNotSupported)) throw new NotSupportedException();
            return !results.Any(x => StatusCode.IsBad(x.Code));
        }

        /// <summary>
        /// Write array values with specialized array node ids and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        private static bool Write<T>(this Session session, (string nodeid, T[] value, string range)[] values)
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.Write(values.Select(x => (new NodeId(x.nodeid), x.value, x.range)).ToArray());
        }

        /// <summary>
        /// Write array value with specialized array node id and index.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, (NodeId node, T value, int index)[] values)
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.Write(values.Select(x => (x.node, new T[] { x.value }, $"{x.index}")).ToArray());
        }

        /// <summary>
        /// Write array value with specialized array node id and index.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, (string nodeId, T value, int index)[] values)
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.Write(values.Select(x => (new NodeId(x.nodeId), new T[] { x.value }, $"{x.index}")).ToArray());
        }

        /// <summary>
        /// Write array value with specialized array node id and index.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the array node.</param>
        /// <param name="values">the node value and index set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, NodeId node, (T value, int index)[] values)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.Write(
                values
                .Select(x => (node, new[] { x.value }, $"{x.index}"))
                .ToArray()
            );
        }

        /// <summary>
        /// Write array value with specialized array node id and index.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the array node id.</param>
        /// <param name="values">the node value and index set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, string nodeId, (T value, int index)[] values)
        {
            if (string.IsNullOrWhiteSpace(nodeId)) throw new ArgumentNullException(nameof(nodeId));
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.Write(
                values
                .Select(x => (new NodeId(nodeId), new[] { x.value }, $"{x.index}"))
                .ToArray()
            );
        }

        /// <summary>
        /// Write array values with specialized node and offset with size.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node.</param>
        /// <param name="values">the value of <see cref="T"/> to be write.</param>
        /// <param name="offset">the offset of array to write.</param>
        /// <param name="size">the size to write,default is 0,the size is using values length.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, NodeId node, T[] values, int offset, int size = 0)
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            if (size > 0 && values.Length < size) throw new ArgumentOutOfRangeException(nameof(size));
            if (size < 1) size = values.Length;
            return session.Write(new (NodeId, T[], string)[]
            {
                (node,values, size > 1 ? $"{offset}:{offset + size - 1}" : $"{offset}")
            });
        }

        /// <summary>
        /// Write array values with specialized node and offset with size.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="values">the value of <see cref="T"/> to be write.</param>
        /// <param name="offset">the offset of array to write.</param>
        /// <param name="size">the size to write,default is 0,the size is using values length.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, string nodeId, T[] values, int offset, int size = 0)
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            if (size > 0 && values.Length < size) throw new ArgumentOutOfRangeException(nameof(size));
            if (size < 1) size = values.Length;
            return session.Write(new (NodeId, T[], string)[]
            {
                (new NodeId(nodeId),values, size > 1 ? $"{offset}:{offset + size - 1}" : $"{offset}")
            });
        }

        /// <summary>
        /// Write array value with specialized array node and offset with size.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node.</param>
        /// <param name="value">the value to be write.</param>
        /// <param name="offset">the offset of array.</param>
        /// <param name="size">the size to write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, NodeId node, T value, int offset, int size = 1)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(offset));
            return session.Write(new (NodeId, T[], string)[]
            {
                (node,Enumerable.Repeat(value,size).ToArray(),  size > 1 ? $"{offset}:{offset + size - 1}" : $"{offset}")
            });
        }

        /// <summary>
        /// Write array value with specialized array node and offset with size.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="value">the value to be write.</param>
        /// <param name="offset">the offset of array to write.</param>
        /// <param name="size">the size to write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, string nodeId, T value, int offset, int size = 1)
        {
            if (string.IsNullOrWhiteSpace(nodeId)) throw new ArgumentNullException(nameof(nodeId));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(offset));
            return session.Write(new (NodeId, T[], string)[]
            {
                (new NodeId(nodeId),Enumerable.Repeat(value,size).ToArray(), size > 1 ? $"{offset}:{offset + size - 1}" : $"{offset}")
            });
        }

        /// <summary>
        /// Write array value with specialized array node and indexs.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node.</param>
        /// <param name="value">the value to be write.</param>
        /// <param name="indexs">the indexs of array.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, NodeId node, T value, int[] indexs)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (indexs == null || indexs.Length == 0) throw new ArgumentNullException(nameof(indexs));

            return session.Write(
                indexs
                .Select(x => (node, new[] { value }, $"{x}"))
                .ToArray()
            );
        }

        /// <summary>
        /// Write array value with specialized array node and indexs.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="value">the value to be write.</param>
        /// <param name="indexs">the indexs of array.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, string nodeId, T value, int[] indexs)
        {
            if (string.IsNullOrWhiteSpace(nodeId)) throw new ArgumentNullException(nameof(nodeId));
            if (indexs == null || indexs.Length == 0) throw new ArgumentNullException(nameof(indexs));
            return session.Write(
                indexs
                .Select(x => (new NodeId(nodeId), new[] { value }, $"{x}"))
                .ToArray()
            );
        }
    }
}