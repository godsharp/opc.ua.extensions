using System;
using System.Collections.Generic;
using System.Linq;
using Opc.Ua;
using Opc.Ua.Client;
using GodSharp.Extensions.Opc.Ua.Types.Encodings;
// ReSharper disable CheckNamespace

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace GodSharp.Extensions.Opc.Ua.Client
{
    // ReSharper disable once UnusedType.Global
    public static partial class SessionExtensions
    {
        /// <summary>
        /// get attributes with specialized node.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the <see cref="NodeId"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<NodeField> GetAttributes(this Session session, NodeId node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            // build list of attributes to read.
            var nodesToRead = new ReadValueIdCollection();
            nodesToRead.AddRange(Attributes.GetIdentifiers().Select(x => new ReadValueId
            {
                NodeId = node,
                AttributeId = x,
                IndexRange = null,
                DataEncoding = null
            }));

            return GetAttributes(session, nodesToRead);
        }

        private static IEnumerable<NodeField> GetAttributes(Session session,ReadValueIdCollection nodesToRead)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            if (nodesToRead == null) throw new ArgumentNullException(nameof(nodesToRead));

            // check for empty list.
            if (nodesToRead.Count == 0) yield return null;

            // read attributes.
            session.Read(
                null,
                0,
                TimestampsToReturn.Neither,
                nodesToRead,
                out var values,
                out var diagnosticInfos);

            ClientBase.ValidateResponse(values, nodesToRead);
            ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

            for (var i = 0; i < nodesToRead.Count; i++)
            {
                // check if node supports attribute.
                if (values[i].StatusCode == StatusCodes.BadAttributeIdInvalid) continue;

                yield return new NodeField(
                    nodesToRead[i],
                    values[i],
                    diagnosticInfos != null && diagnosticInfos.Count > i
                        ? diagnosticInfos[i]
                        : null
                )
                {
                    ValueText = Formatter.FormatAttributeValue(nodesToRead[i].AttributeId, values[i].Value, session)
                };
            }
        }
    }
}