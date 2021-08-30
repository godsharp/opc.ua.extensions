using Opc.Ua;
// ReSharper disable CheckNamespace

namespace GodSharp.Extensions.Opc.Ua.Client
{
    public class NodeField
    {
        public string Name { get; set; }
        public ReadValueId ValueId { get; set; }
        public object Value { get; set; }
        public string ValueText { get; set; }
        public StatusCode StatusCode { get; set; }
        public DiagnosticInfo Diagnostic { get; set; }

        public NodeField()
        {
        }

        public NodeField(ReadValueId readValueId, DataValue dataValue, DiagnosticInfo diagnostic = null)
        {
            Name = Attributes.GetBrowseName(readValueId.AttributeId);
            ValueId = readValueId;
            Value = dataValue.Value;
            StatusCode = dataValue.StatusCode;
            Diagnostic = diagnostic;
        }
    }
}