using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Write values with specialized node ids.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write(this Session session, IEnumerable<(NodeId node, object value)> values)
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

            session.Write(null, nodesToWrite, out var results, out var diagnosticInfos);
            ClientBase.ValidateResponse(results, nodesToWrite);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToWrite);

            return !results.Any(x=>StatusCode.IsBad(x.Code));
        }

        /// <summary>
        /// Write value with specialized node id.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node id.</param>
        /// <param name="value">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, NodeId node, T value)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            return session.Write(new (NodeId node, object value)[] {(node, value)});
        }

        /// <summary>
        /// Write values with specialized node ids.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="values">the node id and value set to be write.</param>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write(this Session session, (string nodeid, object value)[] values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            return session.Write(values.Select(x => (new NodeId(x.nodeid), x.value)));
        }

        /// <summary>
        /// Write value with specialized node id.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="value">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, string nodeId, T value)
        {
            if (nodeId == null) throw new ArgumentNullException(nameof(nodeId));
            return session.Write(new NodeId(nodeId), value);
        }

        /// <summary>
        /// Write enum values with specialized node id.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodeId">the node id.</param>
        /// <param name="values">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, string nodeId, T[] values) where T : Enum
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.Write(nodeId, values.Select(x => Convert.ToInt32(x)).ToArray());
        }

        /// <summary>
        /// Write enum values with specialized node.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the node.</param>
        /// <param name="values">the value of <see cref="T"/> to be write.</param>
        /// <typeparam name="T">the type of value.</typeparam>
        /// <returns>a <see cref="bool"/> result.</returns>
        public static bool Write<T>(this Session session, NodeId node, T[] values) where T : Enum
        {
            if (values == null || values.Length == 0) throw new ArgumentNullException(nameof(values));
            return session.Write(node, values.Select(x => Convert.ToInt32(x)).ToArray());
        }
    }
}