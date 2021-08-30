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
        /// Write value with specialized node id by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node id.</param>
        /// <param name="value">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, NodeId node,T value)
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
                        Value = value, StatusCode = StatusCodes.Good, SourceTimestamp = DateTime.MinValue, ServerTimestamp = DateTime.MinValue
                    }
                }
            });

            var tcs = new TaskCompletionSource<bool>();

            session.BeginWrite(
                null,
                nodesToWrite,
                ar =>
                {
                    try
                    {
                        var response = session.EndWrite(ar, out var results, out _);

                        tcs.TrySetResult(StatusCode.IsGood(response.ServiceResult) && StatusCode.IsGood(results[0].Code));
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
                            Value = x.value, StatusCode = StatusCodes.Good, SourceTimestamp = DateTime.MinValue, ServerTimestamp = DateTime.MinValue
                        }
                    }
                )
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
                        tcs.TrySetResult(StatusCode.IsGood(response.ServiceResult) && !results.Any(x=>StatusCode.IsBad(x.Code)));
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
        /// Write value with specialized node id by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="value">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync<T>(this Session session, string nodeId, T value)
            => session.WriteAsync(new NodeId(nodeId), value);

        /// <summary>
        /// Write values with specialized node ids by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static Task<bool> WriteAsync(this Session session, IEnumerable<(string nodeid, object value)> values)
            => session.WriteAsync(values.Select(x => (new NodeId(x.nodeid), x.value)));


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
    }
}