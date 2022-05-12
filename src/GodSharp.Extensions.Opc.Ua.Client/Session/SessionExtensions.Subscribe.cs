using Opc.Ua;
using Opc.Ua.Client;

using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable CheckNamespace

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global

namespace GodSharp.Extensions.Opc.Ua.Client
{
    public static partial class SessionExtensions
    {
        /// <summary>
        /// Subscribe nodes with specialized parameters
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="name">the name of subscribe group.</param>
        /// <param name="nodes">the set of <see cref="ReferenceDescription"/> node id.</param>
        /// <param name="notificationHandler">the method to callback when monitored node changed.</param>
        /// <param name="configure">the action to fix options for subscription</param>
        /// <param name="samplingInterval">the value of <see cref="MonitoredItem"/> SamplingInterval.</param>
        /// <returns>a <see cref="bool"/> value.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static bool Subscribe(this Session session
            , string name
            , IEnumerable<ReferenceDescription> nodes
            , Action<string, MonitoredItem, MonitoredItemNotificationEventArgs> notificationHandler
            , Action<Subscription> configure = null
            , int samplingInterval = 0)
        {
            var items = nodes?.ToArray();
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (!(nodes != null && items.Any())) throw new ArgumentNullException(nameof(nodes));
            if (notificationHandler == null) throw new ArgumentNullException(nameof(notificationHandler));
            var subscription = session.Subscriptions?.FirstOrDefault(x => name.Equals(x.DisplayName, StringComparison.OrdinalIgnoreCase));

            bool exist;
            if (subscription == null)
            {
                exist = false;
                subscription = new Subscription(session.DefaultSubscription)
                {
                    DisplayName = name,
                    PublishingEnabled = true,
                    PublishingInterval = 0,
                    Priority = 100,
                    KeepAliveCount = uint.MaxValue,
                    LifetimeCount = uint.MaxValue,
                    MaxNotificationsPerPublish = uint.MaxValue
                };

                configure?.Invoke(subscription);
            }
            else
            {
                exist = true;
            }

            foreach (var node in items)
            {
                var item = new MonitoredItem(subscription.DefaultItem)
                {
                    DisplayName = subscription.Session.NodeCache.GetDisplayText(node),
                    StartNodeId = (NodeId)node.NodeId,
                    NodeClass = node.NodeClass,
                    SamplingInterval = samplingInterval,
                    AttributeId = Attributes.Value
                };
                item.Notification += (mi, args) => notificationHandler(name, mi, args);
                subscription.AddItem(item);
            }

            if (exist)
            {
                subscription.ApplyChanges();
                return true;
            }

            session.AddSubscription(subscription);
            subscription.Create();
            return true;
        }

        /// <summary>
        /// Subscribe nodes with specialized parameters
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="name">the name of subscribe group.</param>
        /// <param name="nodes">the set of <see cref="NodeId"/> node id.</param>
        /// <param name="notificationHandler">the method to callback when monitored node changed.</param>
        /// <param name="configure">the action to fix options for subscription</param>
        /// <param name="samplingInterval">the value of <see cref="MonitoredItem"/> SamplingInterval.</param>
        /// <returns>a <see cref="bool"/> value.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static bool Subscribe(this Session session
            , string name
            , IEnumerable<NodeId> nodes
            , Action<string, MonitoredItem, MonitoredItemNotificationEventArgs> notificationHandler
            , Action<Subscription> configure = null
            , int samplingInterval = 0)
        {
            var items = nodes?.ToArray();
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (nodes == null || !items.Any()) throw new ArgumentNullException(nameof(nodes));
            if (notificationHandler == null) throw new ArgumentNullException(nameof(notificationHandler));
            var subscription = session.Subscriptions?.FirstOrDefault(x => name.Equals(x.DisplayName, StringComparison.OrdinalIgnoreCase));

            bool exist;
            if (subscription == null)
            {
                exist = false;
                subscription = new Subscription(session.DefaultSubscription)
                {
                    DisplayName = name,
                    PublishingEnabled = true,
                    PublishingInterval = 0,
                    Priority = 100,
                    KeepAliveCount = uint.MaxValue,
                    LifetimeCount = uint.MaxValue,
                    MaxNotificationsPerPublish = uint.MaxValue
                };

                configure?.Invoke(subscription);
            }
            else
            {
                exist = true;
            }

            foreach (var node in items)
            {
                var item = new MonitoredItem(subscription.DefaultItem)
                {
                    DisplayName = node.Identifier.ToString(),
                    StartNodeId = node,
                    SamplingInterval = samplingInterval,
                    NodeClass = NodeClass.Variable,
                    AttributeId = Attributes.Value
                };
                item.Notification += (mi, args) => notificationHandler(name, mi, args);
                subscription.AddItem(item);
            }

            if (exist)
            {
                subscription.ApplyChanges();
                return true;
            }

            if (session.AddSubscription(subscription))
            {
                session.AddSubscription(subscription);
            }
            else
            {
                if (session.Subscriptions?.Any(x => x.DisplayName == subscription.DisplayName) == true)
                {
                    session.RemoveSubscription(subscription);
                }
                return false;
            }

            subscription.Create();

            return subscription.Created && subscription.Id > 0;
        }

        /// <summary>
        /// Subscribe nodes with specialized parameters
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="name">the name of subscribe group.</param>
        /// <param name="nodeIds">the set of node id.</param>
        /// <param name="notificationHandler">the method to callback when monitored node changed.</param>
        /// <param name="configure">the action to fix options for subscription</param>
        /// <param name="samplingInterval">the value of <see cref="MonitoredItem"/> SamplingInterval.</param>
        /// <returns>a <see cref="bool"/> value.</returns>
        public static bool Subscribe(this Session session
            , string name
            , IEnumerable<string> nodeIds
            , Action<string, MonitoredItem, MonitoredItemNotificationEventArgs> notificationHandler = null
            , Action<Subscription> configure = null
            , int samplingInterval = 0)
            => session.Subscribe(name, nodeIds.Select(x => new NodeId(x)), notificationHandler, configure, samplingInterval);

        /// <summary>
        /// Unsubscribe subscription group with specialized name.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="name">the name of subscribe group.</param>
        /// <returns>a <see cref="bool"/> value.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool Unsubscribe(this Session session, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            var subscription = session.Subscriptions?.FirstOrDefault(x => name.Equals(x.DisplayName, StringComparison.OrdinalIgnoreCase));
            return subscription != null && session.RemoveSubscription(subscription);
        }

        /// <summary>
        /// Unsubscribe items nodes with specialized name.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <param name="name">the name of subscribe group.</param>
        /// <param name="nodes">the set of <see cref="MonitoredItem"/> node id.</param>
        /// <returns>a <see cref="bool"/> value.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool Unsubscribe(this Session session, string name, params string[] nodes)
        {
            var names = nodes;
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (nodes == null || !names.Any()) throw new ArgumentNullException(nameof(nodes));
            var subscription = session.Subscriptions?.FirstOrDefault(x => name.Equals(x.DisplayName, StringComparison.OrdinalIgnoreCase));

            var items = subscription?.MonitoredItems?.Where(
                x => names.Any(
                    a => x.DisplayName == a || (x.StartNodeId.IdType == IdType.String && x.StartNodeId.Identifier?.ToString() == a))
                ).ToArray();

            if (items == null || !items.Any()) return false;
            subscription.RemoveItems(items);
            subscription.ApplyChanges();
            return true;
        }

        /// <summary>
        /// Unsubscribe all groups.
        /// </summary>
        /// <param name="session">the <see cref="Session"/> instance.</param>
        /// <returns>a <see cref="bool"/> value.</returns>
        public static bool UnsubscribeAll(this Session session)
        {
            return session.SubscriptionCount <= 0 || session.RemoveSubscriptions(session.Subscriptions);
        }
    }
}