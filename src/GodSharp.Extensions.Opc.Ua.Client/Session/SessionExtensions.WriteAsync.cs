using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// <summary>
        /// Write values with specialized node ids by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        private static Task<bool> WriteAsync(this Session session, WriteValueCollection values)
        {
            if (values == null || values.Count == 0) throw new ArgumentNullException(nameof(values));

            var tcs = new TaskCompletionSource<bool>();

            session.BeginWrite(
                null,
                values,
                ar =>
                {
                    try
                    {
                        var response = session.EndWrite(ar, out var results, out _);
                        tcs.TrySetResult(StatusCode.IsGood(response.ServiceResult) && !results.Any(x => StatusCode.IsBad(x.Code)));
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                },
                null);

            return tcs.Task;
        }

        /// <summary>
        /// Write values with specialized node ids by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync(this Session session, IEnumerable<(NodeId node, object value)> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

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

            return session.WriteAsync(nodesToWrite);
        }

        /// <summary>
        /// Write values with specialized node ids by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync(this Session session, IEnumerable<(string nodeid, object value)> values)
            => session.WriteAsync(values.Select(x => (new NodeId(x.nodeid), x.value)));

        /// <summary>
        /// Write value with specialized node id by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node.</param>
        /// <param name="value">the value to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync(this Session session, NodeId node, object value)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var nodesToWrite = new WriteValueCollection(new[]
            {
                new WriteValue
                {
                    NodeId = node,
                    AttributeId = Attributes.Value,
                    Value =
                    {
                        Value = value,
                        StatusCode = StatusCodes.Good,
                        SourceTimestamp = DateTime.MinValue,
                        ServerTimestamp = DateTime.MinValue
                    }
                }
            });

            return session.WriteAsync(nodesToWrite);
        }

        /// <summary>
        /// Write value with specialized node id by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="value">the value to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync(this Session session, string nodeId, object value)
            => session.WriteAsync(new NodeId(nodeId), value);

        /// <summary>
        /// Write enum values with specialized node by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node.</param>
        /// <param name="values">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, NodeId node, T[] values) where T : Enum
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.WriteAsync(node, values.Select(x => Convert.ToInt32(x)).ToArray());
        }

        /// <summary>
        /// Write enum values with specialized node id by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="values">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, string nodeId, T[] values) where T : Enum
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.WriteAsync(nodeId, values.Select(x => Convert.ToInt32(x)).ToArray());
        }

        /// <summary>
        /// Write enum value with specialized node id.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node.</param>
        /// <param name="value">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, NodeId node, T value) where T : Enum
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            return session.WriteAsync(node, Convert.ToInt32(value));
        }

        /// <summary>
        /// Write enum value with specialized node id.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="value">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, string nodeId, T value) where T : Enum
        {
            if (string.IsNullOrWhiteSpace(nodeId)) throw new ArgumentNullException(nameof(nodeId));
            return session.WriteAsync(nodeId, Convert.ToInt32(value));
        }

        /// <summary>
        /// Write array values with specialized array node ids and range by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        private static Task<bool> WriteAsync<T>(this Session session, (NodeId node, T[] value, string range)[] values)
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

            var tcs = new TaskCompletionSource<bool>();

            session.BeginWrite(
                null,
                nodesToWrite,
                ar =>
                {
                    try
                    {
                        var response = session.EndWrite(ar, out var results, out _);
                        tcs.TrySetResult(StatusCode.IsGood(response.ServiceResult) && !results.Any(x => StatusCode.IsBad(x.Code)));
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                },
                null);

            return tcs.Task;
        }

        /// <summary>
        /// Write array values with specialized array node ids and range by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        private static Task<bool> WriteAsync<T>(this Session session, (string nodeid, T[] value, string range)[] values)
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.WriteAsync(values.Select(x => (new NodeId(x.nodeid), x.value, x.range)).ToArray());
        }

        /// <summary>
        /// Write array value with specialized array node id and index by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, (NodeId node, T value, int index)[] values)
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.WriteAsync(values.Select(x => (x.node, new T[] { x.value }, $"{x.index}")).ToArray());
        }

        /// <summary>
        /// Write array value with specialized array node id and index by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, (string nodeId, T value, int index)[] values)
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.WriteAsync(values.Select(x => (new NodeId(x.nodeId), new T[] { x.value }, $"{x.index}")).ToArray());
        }

        /// <summary>
        /// Write array value with specialized array node id and index by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the array node.</param>
        /// <param name="values">the node value and index set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, NodeId node, (T value, int index)[] values)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.WriteAsync(new (NodeId, T[], string)[]
            {
                (node,values.Select(x=>x.value).ToArray(),string.Join(",",values.Select(x=>x.index)))
            });
        }

        /// <summary>
        /// Write array value with specialized array node id and index by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the array node id.</param>
        /// <param name="values">the node value and index set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, string nodeId, (T value, int index)[] values)
        {
            if (string.IsNullOrWhiteSpace(nodeId)) throw new ArgumentNullException(nameof(nodeId));
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.WriteAsync(new (NodeId, T[], string)[]
            {
                (new NodeId(nodeId),values.Select(x=>x.value).ToArray(),string.Join(",",values.Select(x=>x.index)))
            });
        }

        /// <summary>
        /// Write array values with specialized node and offset with size by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node.</param>
        /// <param name="values">the value of <see cref="T"/> to be write.</param>
        /// <param name="offset">the offset of array to write.</param>
        /// <param name="size">the size to write,default is 0,the size is using values length.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, NodeId node, T[] values, int offset, int size = 0)
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            if (size > 0 && values.Length < size) throw new ArgumentOutOfRangeException(nameof(size));
            if (size < 1) size = values.Length;
            return session.WriteAsync(new (NodeId, T[], string)[]
            {
                (node,values, size > 1 ? $"{offset}:{offset + size - 1}" : $"{offset}")
            });
        }

        /// <summary>
        /// Write array values with specialized node and offset with size by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="values">the value of <see cref="T"/> to be write.</param>
        /// <param name="offset">the offset of array to write.</param>
        /// <param name="size">the size to write,default is 0,the size is using values length.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, string nodeId, T[] values, int offset, int size = 0)
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            if (size > 0 && values.Length < size) throw new ArgumentOutOfRangeException(nameof(size));
            if (size < 1) size = values.Length;
            return session.WriteAsync(new (NodeId, T[], string)[]
            {
                (new NodeId(nodeId),values, size > 1 ? $"{offset}:{offset + size - 1}" : $"{offset}")
            });
        }

        /// <summary>
        /// Write array value with specialized array node and offset with size by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node.</param>
        /// <param name="value">the value to be write.</param>
        /// <param name="offset">the offset of array.</param>
        /// <param name="size">the size to write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, NodeId node, T value, int offset, int size = 1)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(offset));
            return session.WriteAsync(new (NodeId, T[], string)[]
            {
                (node,Enumerable.Repeat(value,size).ToArray(),  size>1 ? $"{offset}:{offset + size - 1}" : $"{offset}")
            });
        }

        /// <summary>
        /// Write array value with specialized array node and offset with size by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="value">the value to be write.</param>
        /// <param name="offset">the offset of array.</param>
        /// <param name="size">the size to write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, string nodeId, T value, int offset, int size = 1)
        {
            if (string.IsNullOrWhiteSpace(nodeId)) throw new ArgumentNullException(nameof(nodeId));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(offset));
            return session.WriteAsync(new (NodeId, T[], string)[]
            {
                (new NodeId(nodeId),Enumerable.Repeat(value,size).ToArray(), size>1 ? $"{offset}:{offset + size - 1}" : $"{offset}")
            });
        }

        /// <summary>
        /// Write array value with specialized array node and indexs by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node.</param>
        /// <param name="value">the value to be write.</param>
        /// <param name="indexs">the indexs of array.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, NodeId node, T value, int[] indexs)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (indexs == null || indexs.Length == 0) throw new ArgumentNullException(nameof(indexs));
            return session.WriteAsync(
                indexs
                .Select(x => (node, new[] { value }, $"{x}"))
                .ToArray()
            );
        }

        /// <summary>
        /// Write array value with specialized array node and indexs by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="value">the value to be write.</param>
        /// <param name="indexs">the indexs of array.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, string nodeId, T value, int[] indexs)
        {
            if (string.IsNullOrWhiteSpace(nodeId)) throw new ArgumentNullException(nameof(nodeId));
            if (indexs == null || indexs.Length == 0) throw new ArgumentNullException(nameof(indexs));
            return session.WriteAsync(
                indexs
                .Select(x => (new NodeId(nodeId), new[] { value }, $"{x}"))
                .ToArray()
            );
        }
    }
}