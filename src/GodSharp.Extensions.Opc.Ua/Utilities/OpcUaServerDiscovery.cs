using Opc.Ua;

using System;
using System.Collections.Generic;

namespace GodSharp.Extensions.Opc.Ua.Utilities
{
    public class OpcUaServerDiscovery
    {
        public int OperationTimeout { get; set; }= 5000;

        /// <summary>
        /// Discovery opc ua server
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private ApplicationDescription[] DiscoveryInternal(string host)
        {
            //opc.tcp://{0}:4840
            return Run(
                new Uri(host),
                x =>
            {
                var list = new List<ApplicationDescription>();
                try
                {
                    var servers = x.FindServers(null);

                    // populate the drop down list with the discovery URLs for the available servers.
                    // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
                    foreach (var t in servers)
                    {
                        // don't show discovery servers.
                        if (t.ApplicationType == ApplicationType.DiscoveryServer) continue;
                        list.Add(t);
                    }
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception)
                {
                }

                return list.Count == 0 ? null : list.ToArray();
            });
        }

        /// <summary>
        /// Discovery opc ua server
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public ApplicationDescription[] Discovery(string host)
            => DiscoveryInternal(host);

        public string[] DiscoveryUrls(string host)
        {
            var discovery = DiscoveryInternal(host);
            if(discovery==null) return null;
            List<string> urls = new();
            foreach (var applicationDescription in discovery)
            {
                // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
                foreach (var url in applicationDescription.DiscoveryUrls)
                {
                    if (url.EndsWith("/discovery", StringComparison.InvariantCultureIgnoreCase)) continue;
                    if (urls.Contains(url)) continue;
                    urls.Add(url);
                }
            }

            return urls.Count == 0 ? null : urls.ToArray();
        }

        /// <summary>
        /// get endpoints
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public EndpointDescriptionCollection GetEndpoints(string host)
        {
            return Run(Utils.ParseUri(host), x => 
            {
                try
                {
                    return x.GetEndpoints(null);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        private T Run<T>(Uri host, Func<DiscoveryClient, T> func)
        {
            // set a short timeout because this is happening in the drop down event.
            var configuration = EndpointConfiguration.Create();
            configuration.OperationTimeout = OperationTimeout;

            // Connect to the local discovery server and find the available servers.
            using var client = DiscoveryClient.Create(host, configuration);
            return func(client);
        }
    }
}
