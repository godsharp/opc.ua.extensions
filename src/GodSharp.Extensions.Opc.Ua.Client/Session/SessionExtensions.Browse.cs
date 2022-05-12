using GodSharp.Extensions.Opc.Ua.Types.Encodings;

using Opc.Ua;
using Opc.Ua.Client;

using System;
using System.Linq;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable CheckNamespace
// ReSharper disable BitwiseOperatorOnEnumWithoutFlags

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace GodSharp.Extensions.Opc.Ua.Client
{
    public static partial class SessionExtensions
    {
        /// <summary>
        /// Browse node
        /// </summary>
        /// <param name="session"></param>
        /// <param name="node"></param>
        /// <param name="direction">Default is <seealso cref="BrowseDirection.Forward"/></param>
        /// <param name="referenceTypeId">Default is : <seealso cref="ReferenceTypeIds.Organizes"/></param>
        /// <param name="includeSubtype"></param>
        /// <param name="nodeClassMask">Default is : (int) (NodeClass.Variable | NodeClass.Object | NodeClass.Method)</param>
        /// <returns></returns>
        public static ReferenceDescriptionCollection Browse(
            this Session session,
            NodeId node = null,
            BrowseDirection direction = BrowseDirection.Forward,
            NodeId referenceTypeId = null,
            bool includeSubtype = true,
            int? nodeClassMask = null
        )
        {
            var nod = node == null ? Objects.RootFolder : node;
            var browser = new Browser(session)
            {
                BrowseDirection = direction,
                ReferenceTypeId = referenceTypeId ?? ReferenceTypeIds.Organizes,
                IncludeSubtypes = includeSubtype,
                NodeClassMask = nodeClassMask ?? (int)(NodeClass.Object | NodeClass.Variable | NodeClass.Method)
            };

            return browser.Browse(nod);
        }

        /// <summary>
        /// Browse node tree
        /// </summary>
        /// <param name="session"></param>
        /// <param name="node"></param>
        /// <param name="depth">Default is -1,not limit</param>
        /// <param name="direction">Default is <seealso cref="BrowseDirection.Forward"/></param>
        /// <param name="referenceTypeId">Default is : <seealso cref="ReferenceTypeIds.HierarchicalReferences"/></param>
        /// <param name="includeSubtype"></param>
        /// <param name="nodeClassMask">Default is : (int) (NodeClass.Variable | NodeClass.Object | NodeClass.Method)</param>
        /// <returns></returns>
        public static ReferenceBrowseDescription[] BrowseTree(
            this Session session,
            NodeId node = null,
            int depth = -1,
            BrowseDirection direction = BrowseDirection.Forward,
            NodeId referenceTypeId = null,
            bool includeSubtype = true,
            int? nodeClassMask = null
        )
        {
            return BrowseTree(
                session, node == null ? Objects.RootFolder : node,
                0,
                depth,
                direction,
                referenceTypeId ?? ReferenceTypeIds.HierarchicalReferences,
                includeSubtype,
                nodeClassMask ?? (int)(NodeClass.Variable | NodeClass.Object | NodeClass.Method)
            );
        }

        private static ReferenceBrowseDescription[] BrowseTree(
            Session session,
            NodeId node,
            int current,
            int depth,
            BrowseDirection direction,
            NodeId referenceTypeId,
            bool includeSubtype,
            int nodeClassMask
        )
        {
            if (depth > -1 && depth <= current) return null;

            var browser = new Browser(session)
            {
                BrowseDirection = direction,
                ReferenceTypeId = referenceTypeId,
                IncludeSubtypes = includeSubtype,
                //NodeClassMask =  (int)BrowseResultMask.All
                NodeClassMask = nodeClassMask // (int) (NodeClass.Variable | NodeClass.Object | NodeClass.Method)
            };

            try
            {
                var refs = browser.Browse(node);

                return refs
                    ?.Select(rd => new ReferenceBrowseDescription(
                            rd,
                            BrowseTree(
                                session,
                                (NodeId)rd.NodeId,
                                current,
                                depth,
                                direction,
                                referenceTypeId,
                                includeSubtype,
                                nodeClassMask
                            ),
                            current++
                        )
                    ).ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public class ReferenceBrowseDescription
    {
        public ReferenceDescription Node { get; }
        public int Depth { get; }
        public ReferenceBrowseDescription[] Children { get; }

        public ReferenceBrowseDescription(ReferenceDescription node, ReferenceBrowseDescription[] children, int depth = 0)
        {
            Node = node ?? throw new ArgumentNullException(nameof(node));
            Children = children;
            Depth = depth;
        }
    }

    public static class ReferenceBrowseDescriptionExtensions
    {
        public static string GetFormatText(this ReferenceDescription description) => Formatter.FormatReferenceDescriptionText(description);

        public static string GetFormatText(this ReferenceBrowseDescription description) => description.Node?.GetFormatText();
    }
}