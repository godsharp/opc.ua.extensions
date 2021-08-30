using System;
using System.Linq;
using System.Threading.Tasks;
using Opc.Ua;
using Opc.Ua.Client;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable CheckNamespace

namespace GodSharp.Extensions.Opc.Ua.Client
{
    public static partial class SessionExtensions
    {
        /// <summary>
        /// Register nodes by asynchronous,from ns=3;s=bool to ns=9;i=1.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the nodes.</param>
        /// <returns>return <seealso cref="NodeId"/> array.</returns>
        public static Task<NodeId[]> RegisterNodesAsync(this Session session, params NodeId[] nodes)
        {
            if (nodes.Length == 0) return Task.FromResult<NodeId[]>(null);
            var tcs = new TaskCompletionSource<NodeId[]>();
            session.BeginRegisterNodes(
                null,
                nodes,
                ar =>
                {
                    try
                    {
                        var response = session.EndRegisterNodes(
                            ar,
                            out var results
                        );

                        if (StatusCode.IsGood(response.ServiceResult)) tcs.TrySetResult(results.ToArray());
                        else tcs.TrySetException(new Exception($"async register nodes failed:{response.ServiceResult}"));
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                },
                null
            );

            return tcs.Task;
        }

        /// <summary>
        /// Register nodes by asynchronous,from ns=3;s=bool to ns=9;i=1.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the nodes.</param>
        /// <returns>return <seealso cref="NodeId"/> array.</returns>
        public static Task<NodeId[]> RegisterNodesAsync(this Session session, params string[] nodes)
            => session.RegisterNodesAsync(nodes?.Select(x => new NodeId(x)).ToArray());

        /// <summary>
        /// Unregister nodes by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the nodes.</param>
        /// <returns>a bool.</returns>
        public static Task<bool> UnregisterNodesAsync(this Session session, params NodeId[] nodes)
        {
            if (nodes.Length == 0) return Task.FromResult(true);

            var tcs = new TaskCompletionSource<bool>();
            session.BeginUnregisterNodes(
                null,
                nodes,
                ar =>
                {
                    try
                    {
                        var response = session.EndUnregisterNodes(
                            ar
                        );

                        if (StatusCode.IsGood(response.ServiceResult)) tcs.TrySetResult(true);
                        else tcs.TrySetException(new Exception($"async unregister nodes failed:{response.ServiceResult}"));
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                },
                null
            );

            return tcs.Task;
        }

        /// <summary>
        /// Unregister nodes by asynchronous.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="nodes">the nodes.</param>
        /// <returns>a bool.</returns>
        public static Task<bool> UnregisterNodesAsync(this Session session, params string[] nodes)
            => session.UnregisterNodesAsync(nodes?.Select(x => new NodeId(x)).ToArray());
    }
}