using System;
using System.Collections.Generic;
using System.Linq;
using Opc.Ua;
using Opc.Ua.Client;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable CheckNamespace

namespace GodSharp.Extensions.Opc.Ua.Client
{
    // ReSharper disable once UnusedType.Global
    public static partial class SessionExtensions
    {
        
        /// <summary>
        /// get properties with specialized node.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="node">the <see cref="NodeId"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<NodeField> GetProperties(this Session session, NodeId node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            var nodesToRead = new ReadValueIdCollection();

            var browser = new Browser(session)
            {
                BrowseDirection = BrowseDirection.Forward,
                ReferenceTypeId = ReferenceTypeIds.HasProperty,
                IncludeSubtypes = true,
                NodeClassMask = (int) NodeClass.Variable,
                ContinueUntilDone = true
            };
            
            var references = browser.Browse(node);

            if (references == null || references.Count == 0) return null;

            // build list of attributes to read.
            nodesToRead.AddRange(references.Select(x => new ReadValueId
            {
                NodeId = (NodeId) x.NodeId,
                AttributeId = Attributes.Value,
                IndexRange = null,
                DataEncoding = null
            }));

            return GetAttributes(session, nodesToRead);
        }
    }
}