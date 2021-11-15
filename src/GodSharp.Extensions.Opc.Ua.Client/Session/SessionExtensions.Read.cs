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
        /// Read values with specialized node and range set.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the set of <see cref="NodeId"/> and <see cref="NumericRange"/> kv.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<DataValue> Read(this Session session, IEnumerable<(NodeId Node, NumericRange Range)> nodes)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));

            var nodesToRead = new ReadValueIdCollection(
                nodes
                    .Select(x => new ReadValueId
                    {
                        NodeId = x.Node,
                        AttributeId = Attributes.Value,
                        ParsedIndexRange = x.Range
                    })
                );

            session.Read(null, 0, TimestampsToReturn.Both, nodesToRead, out var results, out var diagnosticInfos);
            ClientBase.ValidateResponse(results, nodesToRead);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

            return results;
        }

        /// <summary>
        /// Read values with specialized node and range set.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the set of <see cref="NodeId"/> and <see cref="NumericRange"/> kv.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <typeparam name="T">the value type.</typeparam>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> Read<T>(this Session session, IEnumerable<(NodeId Node, NumericRange Range)> nodes, T defaultValue = default)
            => session.Read(nodes)?.Select(x => x.GetValue(defaultValue));

        /// <summary>
        /// Read values with specialized node and range set.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the set of <see cref="string"/> and <see cref="NumericRange"/> kv.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<DataValue> Read(this Session session, IEnumerable<(string Node, NumericRange Range)> nodes)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));

            return session.Read(nodes.Select(x => (new NodeId(x.Node), x.Range)));
        }

        /// <summary>
        /// Read values with specialized node and range set.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the set of <see cref="string"/> and <see cref="NumericRange"/> kv.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <typeparam name="T">the value type.</typeparam>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> Read<T>(this Session session, IEnumerable<(string Node, NumericRange Range)> nodes, T defaultValue = default)
            => session.Read(nodes)?.Select(x => x.GetValue(defaultValue));

        /// <summary>
        /// Read values with specialized node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="NodeId"/>.</param>
        /// <param name="range">the range of <see cref="NumericRange"/>.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<DataValue> Read(this Session session, NodeId node, NumericRange range)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            return session.Read(new[] { (node, range) });
        }

        /// <summary>
        /// Read values with specialized node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="NodeId"/>.</param>
        /// <param name="range">the range of <see cref="NumericRange"/>.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> Read<T>(this Session session, NodeId node, NumericRange range, T defaultValue = default)
            => session.Read(node, range)?.Select(x => x.GetValue(defaultValue));

        /// <summary>
        /// Read values with specialized node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="string"/>.</param>
        /// <param name="range">the range of <see cref="NumericRange"/>.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<DataValue> Read(this Session session, string node, NumericRange range)
        {
            if (string.IsNullOrWhiteSpace(node)) throw new ArgumentNullException(nameof(node));

            return session.Read(new[] { (new NodeId(node), range) });
        }

        /// <summary>
        /// Read values with specialized node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="string"/>.</param>
        /// <param name="range">the range of <see cref="NumericRange"/>.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<T> Read<T>(this Session session, string node, NumericRange range, T defaultValue = default)
        {
            if (string.IsNullOrWhiteSpace(node)) throw new ArgumentNullException(nameof(node));
            return session.Read(new NodeId(node), range)?.Select(x => x.GetValue(defaultValue));
        }

        /// <summary>
        /// Read values with specialized node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="NodeId"/>.</param>
        /// <param name="offset">the offset of array.</param>
        /// <param name="size">the size to read,default is -1:read to end.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<DataValue> Read(this Session session, NodeId node, int offset, int size = -1)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));

            return session.Read(new[] { (node, new NumericRange(offset, size < 1 ? -1 : offset + size)) });
        }

        /// <summary>
        /// Read values with specialized node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="NodeId"/>.</param>
        /// <param name="offset">the offset of array.</param>
        /// <param name="size">the size to read,default is -1:read to end.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<T> Read<T>(this Session session, NodeId node, int offset, int size = -1, T defaultValue = default)
            => session.Read(new NodeId(node), offset, size, defaultValue);

        /// <summary>
        /// Read values with specialized node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="string"/>.</param>
        /// <param name="offset">the offset of array.</param>
        /// <param name="size">the size to read,default is -1:read to end.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<DataValue> Read(this Session session, string node, int offset, int size = -1)
        {
            if (string.IsNullOrWhiteSpace(node)) throw new ArgumentNullException(nameof(node));

            return session.Read(new NodeId(node), offset, size);
        }

        /// <summary>
        /// Read values with specialized node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="string"/>.</param>
        /// <param name="offset">the offset of array.</param>
        /// <param name="size">the size to read,default is -1:read to end.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<T> Read<T>(this Session session, string node, int offset, int size = -1, T defaultValue = default)
        {
            if (string.IsNullOrWhiteSpace(node)) throw new ArgumentNullException(nameof(node));

            return session.Read(new NodeId(node), offset, size, defaultValue);
        }

        /// <summary>
        /// Read values with specialized node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="NodeId"/>.</param>
        /// <param name="indexs">the set of index to read.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<DataValue> Read(this Session session, NodeId node, int[] indexs)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (indexs == null) throw new ArgumentNullException(nameof(indexs));

            return session.Read(new[]
            {
                (   node,
                    new NumericRange()
                    {
                        SubRanges = indexs.Select(x => new NumericRange(x, x + 1)).ToArray()
                    }
                )
            });
        }

        /// <summary>
        /// Read values with specialized node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="NodeId"/>.</param>
        /// <param name="indexs">the set of index to read.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<T> Read<T>(this Session session, NodeId node, int[] indexs, T defaultValue = default)
            => session.Read(new NodeId(node), indexs, defaultValue);

        /// <summary>
        /// Read values with specialized node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="string"/>.</param>
        /// <param name="indexs">the set of index to read.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<DataValue> Read(this Session session, string node, int[] indexs)
        {
            if (string.IsNullOrWhiteSpace(node)) throw new ArgumentNullException(nameof(node));
            return session.Read(new NodeId(node), indexs);
        }

        /// <summary>
        /// Read values with specialized node and range.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node of <see cref="string"/>.</param>
        /// <param name="indexs">the set of index to read.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<T> Read<T>(this Session session, string node, int[] indexs, T defaultValue = default)
        {
            if (string.IsNullOrWhiteSpace(node)) throw new ArgumentNullException(nameof(node));
            return session.Read(new NodeId(node), indexs, defaultValue);
        }
    }
}