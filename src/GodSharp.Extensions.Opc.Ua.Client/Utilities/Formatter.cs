using System;
using System.Text;
using Opc.Ua;
using Opc.Ua.Client;

namespace GodSharp.Extensions.Opc.Ua.Types.Encodings
{
    public static class Formatter
    {
        /// <summary>
        /// get display text
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static string FormatReferenceDescriptionText(ReferenceDescription reference)
        {
            if (reference == null) return null;
            if (reference.DisplayName != null && !string.IsNullOrEmpty(reference.DisplayName.Text))
            {
                return reference.DisplayName.Text;
            }
            return reference.BrowseName != null ? reference.BrowseName.Name : null;
        }

        /// <summary>
        /// Formats the value of an attribute.
        /// </summary>
        public static string FormatAttributeValue(uint attributeId, object value, Session session = null)
        {
            switch (attributeId)
            {
                case Attributes.NodeClass:
                {
                    return value != null ? $"{Enum.ToObject(typeof(NodeClass), value)}" : "(null)";
                }
                case Attributes.DataType:
                {
                    var datatypeId = value as NodeId;
                    if (datatypeId == null || session == null) return $"{value}";
                    var datatype = session.NodeCache.Find(datatypeId);
                    return datatype == null ? $"{datatypeId}" : datatype.DisplayName.Text;
                }
                case Attributes.ValueRank:
                {
                    if (value is not int valueRank) return $"{value}";
                    return valueRank switch
                    {
                        ValueRanks.Scalar => "Scalar",
                        ValueRanks.OneDimension => "OneDimension",
                        ValueRanks.OneOrMoreDimensions => "OneOrMoreDimensions",
                        ValueRanks.Any => "Any",
                        _ => $"{valueRank}"
                    };
                }
                case Attributes.MinimumSamplingInterval:
                {
                    var minimumSamplingInterval = value as double?;
                    return minimumSamplingInterval switch
                    {
                        null => $"{value}",
                        MinimumSamplingIntervals.Indeterminate => "Indeterminate",
                        MinimumSamplingIntervals.Continuous => "Continuous",
                        _ => $"{minimumSamplingInterval.Value}"
                    };
                }
                case Attributes.AccessLevel:
                case Attributes.UserAccessLevel:
                {
                    var accessLevel = Convert.ToByte(value);
                    var bits = new StringBuilder();
                    if ((accessLevel & AccessLevels.CurrentRead) != 0)
                    {
                        bits.Append("Readable");
                    }

                    if ((accessLevel & AccessLevels.CurrentWrite) != 0)
                    {
                        if (bits.Length > 0)
                        {
                            bits.Append(" | ");
                        }

                        bits.Append("Writeable");
                    }

                    if ((accessLevel & AccessLevels.HistoryRead) != 0)
                    {
                        if (bits.Length > 0)
                        {
                            bits.Append(" | ");
                        }

                        bits.Append("History");
                    }

                    if ((accessLevel & AccessLevels.HistoryWrite) != 0)
                    {
                        if (bits.Length > 0)
                        {
                            bits.Append(" | ");
                        }

                        bits.Append("History Update");
                    }

                    if (bits.Length == 0)
                    {
                        bits.Append("No Access");
                    }

                    return $"{bits}";
                }
                case Attributes.EventNotifier:
                {
                    var notifier = Convert.ToByte(value);
                    var bits = new StringBuilder();
                    if ((notifier & EventNotifiers.SubscribeToEvents) != 0)
                    {
                        bits.Append("Subscribe");
                    }

                    if ((notifier & EventNotifiers.HistoryRead) != 0)
                    {
                        if (bits.Length > 0)
                        {
                            bits.Append(" | ");
                        }

                        bits.Append("History");
                    }

                    if ((notifier & EventNotifiers.HistoryWrite) != 0)
                    {
                        if (bits.Length > 0)
                        {
                            bits.Append(" | ");
                        }

                        bits.Append("History Update");
                    }

                    if (bits.Length == 0)
                    {
                        bits.Append("No Access");
                    }

                    return $"{bits}";
                }
                default:
                {
                    return $"{value}";
                }
            }
        }
    }
}