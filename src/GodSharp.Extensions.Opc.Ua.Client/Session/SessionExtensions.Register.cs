using System.Linq;
using Opc.Ua;
using Opc.Ua.Client;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace GodSharp.Extensions.Opc.Ua.Client
{
    public static partial class SessionExtensions
    {
        /// <summary>
        /// Register nodes,from ns=3;s=bool to ns=9;i=1.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the nodes.</param>
        /// <returns>return <seealso cref="NodeId"/> array.</returns>
        public static NodeId[] RegisterNodes(this Session session, params NodeId[] nodes)
        {
            if (nodes.Length == 0) return null;
            session.RegisterNodes(null, nodes, out var ids);
            return ids?.ToArray();
        }

        /// <summary>
        /// Register nodes,from ns=3;s=bool to ns=9;i=1.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the nodes.</param>
        /// <returns>return <seealso cref="NodeId"/> array.</returns>
        public static NodeId[] RegisterNodes(this Session session, params string[] nodes)
            => session.RegisterNodes(nodes?.Select(x => new NodeId(x)).ToArray());

        /// <summary>
        /// Unregister nodes
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the nodes.</param>
        /// <returns>a bool.</returns>
        public static bool UnregisterNodes(this Session session, params NodeId[] nodes)
        {
            if (nodes.Length == 0) return true;
            var response = session.UnregisterNodes(null, nodes);
            return StatusCode.IsGood(response.ServiceResult);
        }

        /// <summary>
        /// Unregister nodes
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the nodes.</param>
        /// <returns>a bool.</returns>
        public static bool UnregisterNodes(this Session session, params string[] nodes)
            => session.UnregisterNodes(nodes?.Select(x => new NodeId(x)).ToArray());
    }
}