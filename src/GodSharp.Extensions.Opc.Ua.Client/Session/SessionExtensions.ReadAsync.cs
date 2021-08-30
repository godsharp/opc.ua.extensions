using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Opc.Ua;
using Opc.Ua.Client;
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CheckNamespace

namespace GodSharp.Extensions.Opc.Ua.Client
{
    public static partial class SessionExtensions
    {
        /// <summary>
        /// Read values with specialized node set by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the set of <see cref="NodeId"/>.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task<IEnumerable<DataValue>> ReadAsync(this Session session, IEnumerable<NodeId> nodes)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));

            var nodesToRead = new ReadValueIdCollection(nodes.Select(node => new ReadValueId {NodeId = node, AttributeId = Attributes.Value}));
            var tcs = new TaskCompletionSource<IEnumerable<DataValue>>();

            session.BeginRead(
                null,
                0,
                TimestampsToReturn.Neither,
                nodesToRead,
                ar =>
                {
                    try
                    {
                        var response = session.EndRead(
                            ar,
                            out var results,
                            out _);

                        if (StatusCode.IsGood(response.ServiceResult)) tcs.TrySetResult(results);
                        else tcs.TrySetException(new Exception($"async read nodes failed:{response.ServiceResult}"));
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
        /// Read values with specialized node set by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the set of <see cref="NodeId"/>.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <typeparam name="T">the value type.</typeparam>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task<IEnumerable<T>> ReadAsync<T>(this Session session, IEnumerable<NodeId> nodes,T defaultValue=default)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));

            var nodesToRead = new ReadValueIdCollection(nodes.Select(node => new ReadValueId {NodeId = node, AttributeId = Attributes.Value}));
            var tcs = new TaskCompletionSource<IEnumerable<T>>();

            session.BeginRead(
                null,
                0,
                TimestampsToReturn.Neither,
                nodesToRead,
                ar =>
                {
                    try
                    {
                        var response = session.EndRead(
                            ar,
                            out var results,
                            out _);

                        if (StatusCode.IsGood(response.ServiceResult)) tcs.TrySetResult(results.Select(x => x.ValueOf(defaultValue)));
                        else tcs.TrySetException(new Exception($"async read nodes failed:{response.ServiceResult}"));
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
        /// Read value with specialized node by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the <see cref="NodeId"/>.</param>
        /// <returns>a value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task<DataValue> ReadAsync(this Session session, NodeId node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var nodesToRead = new ReadValueIdCollection(new[] {new ReadValueId {NodeId = node, AttributeId = Attributes.Value}});

            var tcs = new TaskCompletionSource<DataValue>();

            session.BeginRead(
                null,
                0,
                TimestampsToReturn.Neither,
                nodesToRead,
                ar =>
                {
                    try
                    {
                        var response = session.EndRead(
                            ar,
                            out var results,
                            out _);

                        if (StatusCode.IsGood(response.ServiceResult)) tcs.TrySetResult(results.FirstOrDefault());
                        else tcs.TrySetException(new Exception($"async read node failed:{response.ServiceResult}"));
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
        /// Read value with specialized node by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the <see cref="NodeId"/>.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <typeparam name="T">the value type.</typeparam>
        /// <returns>a value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task<T> ReadAsync<T>(this Session session, NodeId node,T defaultValue=default)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var nodesToRead = new ReadValueIdCollection(new[] {new ReadValueId {NodeId = node, AttributeId = Attributes.Value}});

            var tcs = new TaskCompletionSource<T>();

            session.BeginRead(
                null,
                0,
                TimestampsToReturn.Neither,
                nodesToRead,
                ar =>
                {
                    try
                    {
                        var response = session.EndRead(
                            ar,
                            out var results,
                            out _);

                        if (StatusCode.IsGood(response.ServiceResult)) tcs.TrySetResult(results[0].ValueOf(defaultValue));
                        else tcs.TrySetException(new Exception($"async read node failed:{response.ServiceResult}"));
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
        /// Read values with specialized node id set by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeIds">the set of node id.</param>
        /// <returns>an set value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task<IEnumerable<DataValue>> ReadAsync(this Session session, IEnumerable<string> nodeIds)
        {
            if (nodeIds == null) throw new ArgumentNullException(nameof(nodeIds));
            return session.ReadAsync(nodeIds.Select(x => new NodeId(x)));
        }

        /// <summary>
        /// Read values with specialized node id set by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeIds">the set of node id.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <typeparam name="T">the value type.</typeparam>
        /// <returns>an set value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task<IEnumerable<T>> ReadAsync<T>(this Session session, IEnumerable<string> nodeIds,T defaultValue=default)
        {
            if (nodeIds == null) throw new ArgumentNullException(nameof(nodeIds));
            return session.ReadAsync(nodeIds.Select(x => new NodeId(x)), defaultValue);
        }

        /// <summary>
        /// Read value with specialized node id by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <returns>a value of <see cref="DataValue"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task<DataValue> ReadAsync(this Session session, string nodeId)
        {
            if (nodeId == null) throw new ArgumentNullException(nameof(nodeId));
            return session.ReadAsync(new NodeId(nodeId));
        }

        /// <summary>
        /// Read value with specialized node id by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="defaultValue">the default value to return when read failed.</param>
        /// <typeparam name="T">the value type.</typeparam>
        /// <returns>a value of <see cref="T"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task<T> ReadAsync<T>(this Session session, string nodeId,T defaultValue=default)
        {
            if (nodeId == null) throw new ArgumentNullException(nameof(nodeId));
            return session.ReadAsync(new NodeId(nodeId),defaultValue);
        }
    }
}