using System;
using System.Collections.Generic;
using System.Linq;

using Opc.Ua;
using Opc.Ua.Client;
// ReSharper disable UnusedMember.Global
// ReSharper disable CheckNamespace

namespace GodSharp.Extensions.Opc.Ua.Client
{
    public static partial class SessionExtensions
    {
        /// <summary>
        /// Read values with specialized node set.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the set of <see cref="NodeId"/>.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<DataValue> Read(this Session session, IEnumerable<NodeId> nodes)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));

            var nodesToRead = new ReadValueIdCollection(nodes.Select(node => new ReadValueId { NodeId = node, AttributeId = Attributes.Value }));

            session.Read(null, 0, TimestampsToReturn.Both, nodesToRead, out var results, out var diagnosticInfos);
            ClientBase.ValidateResponse(results, nodesToRead);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

            return results;
        }

        /// <summary>
        /// Read values with specialized node set.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the set of <see cref="NodeId"/>.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <typeparam name="T">the value type.</typeparam>
        /// <returns>an set value of <see cref="T"/>.</returns>
        public static IEnumerable<T> Read<T>(this Session session, IEnumerable<NodeId> nodes, T defaultValue = default)
            => session.Read(nodes)?.Select(x => x.GetValue(defaultValue));

        /// <summary>
        /// Read value with specialized node.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the <see cref="NodeId"/>.</param>
        /// <returns>a value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DataValue Read(this Session session, NodeId node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            return session.Read(new[] { node })?.FirstOrDefault();
        }

        /// <summary>
        /// Read value with specialized node.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the <see cref="NodeId"/>.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <typeparam name="T">the value type.</typeparam>
        /// <returns>a value of <see cref="T"/>.</returns>
        public static T Read<T>(this Session session, NodeId node, T defaultValue = default)
            => session.Read(node).ValueOf(defaultValue);

        /// <summary>
        /// Read values with specialized node id set.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeIds">the set of node id.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<DataValue> Read(this Session session, IEnumerable<string> nodeIds)
        {
            if (nodeIds == null) throw new ArgumentNullException(nameof(nodeIds));
            return session.Read(nodeIds.Select(x => new NodeId(x)));
        }

        /// <summary>
        /// Read values with specialized node id set.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeIds">the set of node id.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <typeparam name="T">the value type.</typeparam>
        /// <returns>an set value of <see cref="T"/>.</returns>
        public static IEnumerable<T> Read<T>(this Session session, IEnumerable<string> nodeIds, T defaultValue = default)
            => session.Read(nodeIds)?.Select(x => x.GetValue(defaultValue));

        /// <summary>
        /// Read value with specialized node id.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <returns>a value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DataValue Read(this Session session, string nodeId)
        {
            if (nodeId == null) throw new ArgumentNullException(nameof(nodeId));
            return session.Read(new NodeId(nodeId));
        }

        /// <summary>
        /// Read value with specialized node id.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <typeparam name="T">the value type.</typeparam>
        /// <returns>a value of <see cref="T"/>.</returns>
        public static T Read<T>(this Session session, string nodeId, T defaultValue = default)
            => session.Read(nodeId).ValueOf(defaultValue);

        /// <summary>
        /// Read array values with specialized array node and range set.
        /// <br/>range is 0:1 or 0,2,4
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the set of <see cref="NodeId"/> and <see cref="string"/> range kv.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static IEnumerable<DataValue> Read(this Session session, params (NodeId Node, string Range)[] nodes)
        {
            if (nodes.Length==0) throw new ArgumentNullException(nameof(nodes));

            var nodesToRead = new ReadValueIdCollection(
                nodes
                    .Select(x => new ReadValueId
                    {
                        NodeId = x.Node,
                        AttributeId = Attributes.Value,
                        IndexRange = x.Range
                    })
                );

            session.Read(null, 0, TimestampsToReturn.Both, nodesToRead, out var results, out var diagnosticInfos);
            ClientBase.ValidateResponse(results, nodesToRead);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

            return results;
        }

        /// <summary>
        /// Read array values with specialized array node and range set.
        /// <br/>range is 0:1 or 0,2,4
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the set of <see cref="NodeId"/> and <see cref="string"/> range kv.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <typeparam name="T">the value type.</typeparam>
        /// <returns>an set value of <see cref="T"/> array.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static IEnumerable<T[]> Read<T>(this Session session, (NodeId Node, string Range)[] nodes, T[] defaultValue = default)
            => session.Read(nodes)?.Select(x => x.ValueOf(defaultValue));

        /// <summary>
        /// Read array values with specialized array node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="NodeId"/>.</param>
        /// <param name="offset">the offset of array.</param>
        /// <param name="size">the size to read,default is 1.</param>
        /// <returns>an value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static DataValue Read(this Session session, NodeId node, int offset, int size = 1)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(size));
            return session.Read(new[] { (node, size > 1 ? $"{offset}:{offset + size - 1}" : $"{offset}") })?.FirstOrDefault();
        }

        /// <summary>
        /// Read array values with specialized array node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="NodeId"/>.</param>
        /// <param name="offset">the offset of array.</param>
        /// <param name="size">the size to read,default is 1.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<T> Read<T>(this Session session, NodeId node, int offset, int size = 1, T[] defaultValue = default)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(size));
            return session.Read(new[] { (node, size > 1 ? $"{offset}:{offset + size - 1}" : $"{offset}") }, defaultValue)?.FirstOrDefault();
        }

        /// <summary>
        /// Read array values with specialized array node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="string"/>.</param>
        /// <param name="offset">the offset of array.</param>
        /// <param name="size">the size to read,default is -1:read to end.</param>
        /// <returns>an value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DataValue Read(this Session session, string node, int offset, int size = 1)
        {
            if (string.IsNullOrWhiteSpace(node)) throw new ArgumentNullException(nameof(node));
            return session.Read(new NodeId(node), offset, size);
        }

        /// <summary>
        /// Read array values with specialized array node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="string"/>.</param>
        /// <param name="offset">the offset of array.</param>
        /// <param name="size">the size to read,default is 1.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> Read<T>(this Session session, string node, int offset, int size = 1, T[] defaultValue = default)
        {
            if (string.IsNullOrWhiteSpace(node)) throw new ArgumentNullException(nameof(node));
            return session.Read(new NodeId(node), offset, size, defaultValue);
        }

        /// <summary>
        /// Read array value with specialized array node and index.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="NodeId"/>.</param>
        /// <param name="index">the index to read.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DataValue Read(this Session session, NodeId node, int index)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (index < 0) throw new ArgumentNullException(nameof(index));
            return session.Read(new[] { (node, $"{index}") }).FirstOrDefault();
        }

        /// <summary>
        /// Read array value with specialized array node and index.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="NodeId"/>.</param>
        /// <param name="index">the index to read.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T Read<T>(this Session session, NodeId node, int index, T defaultValue = default)
            => session.Read(node, index)
            .ValueOf(default(T[]))
            .FirstOrDefault() ?? defaultValue;

        /// <summary>
        /// Read array value with specialized array node and index.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="string"/>.</param>
        /// <param name="index">the index to read.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DataValue Read(this Session session, string node, int index)
        {
            if (string.IsNullOrWhiteSpace(node)) throw new ArgumentNullException(nameof(node));
            if (index < 0) throw new ArgumentNullException(nameof(index));
            return session.Read(new NodeId(node),index);
        }

        /// <summary>
        /// Read array value with specialized array node and index.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="string"/>.</param>
        /// <param name="index">the index to read.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T Read<T>(this Session session, string node, int index, T defaultValue = default)
        {
            if (string.IsNullOrWhiteSpace(node)) throw new ArgumentNullException(nameof(node));
            if (index < 0) throw new ArgumentNullException(nameof(index));
            return session.Read(new NodeId(node), index)
                .ValueOf(default(T[]))
                .FirstOrDefault() ?? defaultValue;
        }

        /// <summary>
        /// Read array value with specialized array node and index.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="NodeId"/>.</param>
        /// <param name="indexs">the indexs of array.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<DataValue> Read(this Session session, NodeId node, int[] indexs)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (indexs == null || indexs.Length == 0) throw new ArgumentNullException(nameof(indexs));
            return session.Read(
                indexs
                .Select(x => (node, $"{x}"))
                .ToArray()
            );
        }

        /// <summary>
        /// Read array value with specialized array node and index.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="string"/>.</param>
        /// <param name="indexs">the indexs of array.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<DataValue> Read(this Session session, string node, int[] indexs)
        {
            if (string.IsNullOrWhiteSpace(node)) throw new ArgumentNullException(nameof(node));
            if (indexs == null || indexs.Length == 0) throw new ArgumentNullException(nameof(indexs));
            return session.Read(
                indexs
                .Select(x => (new NodeId(node), $"{x}"))
                .ToArray()
            );
        }

        /// <summary>
        /// Read array value with specialized array node and index.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="NodeId"/>.</param>
        /// <param name="indexs">the indexs of array.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> Read<T>(this Session session, NodeId node, int[] indexs, T defaultValue = default)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (indexs == null || indexs.Length == 0) throw new ArgumentNullException(nameof(indexs));
            return session.Read(
                indexs
                .Select(x => (node, $"{x}"))
                .ToArray()
            ).Select(x => x.ValueOf(default(T[])).FirstOrDefault() ?? defaultValue);
        }

        /// <summary>
        /// Read array value with specialized array node and index.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="string"/>.</param>
        /// <param name="indexs">the indexs of array.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> Read<T>(this Session session, string node, int[] indexs, T defaultValue = default)
        {
            if (string.IsNullOrWhiteSpace(node)) throw new ArgumentNullException(nameof(node));
            if (indexs == null || indexs.Length == 0) throw new ArgumentNullException(nameof(indexs));
            return session.Read(
                indexs
                .Select(x => (new NodeId(node), $"{x}"))
                .ToArray()
            ).Select(x => x.ValueOf(default(T[])).FirstOrDefault() ?? defaultValue);
        }
    }
}