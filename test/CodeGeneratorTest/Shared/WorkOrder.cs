using System;
using GodSharp.Extensions.Opc.Ua.Types;
//using static GodSharp.Extensions.Opc.Ua.Types.Encodings.EncodingFactory;

using Opc.Ua;

namespace CodeGeneratorMsBuildTest
{
    [ComplexObjectGenerator]
    public partial class WorkOrder : ComplexObject
    {
        public Guid Id;
        public string AssetId;
        public DateTime StartTime;
        public int NoOfStatusComments => StatusComments?.Length ?? 0;
        public WorkOrderStatus[] StatusComments;

        public WorkOrder()
        {
            TypeIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=3003";
            BinaryEncodingIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=5013";
        }

        //public override void Decode(IDecoder decoder)
        //{
        //    //base.Decode(decoder);
        //    Encoding.Read(decoder, ref Id, nameof(Id));
        //    Encoding.Read(decoder, ref AssetId, nameof(AssetId));
        //    Encoding.Read(decoder, ref StartTime, nameof(StartTime));
        //    Encoding.Read(decoder, ref StatusComments, nameof(StatusComments));
        //}
        //
        // public override void Encode(IEncoder encoder)
        // {
        //     EncodingFactory.Encoding.Write(encoder,Id, nameof(Id));
        //     EncodingFactory.Encoding.Write(encoder,AssetId, nameof(AssetId));
        //     EncodingFactory.Encoding.Write(encoder,StartTime, nameof(StartTime));
        //     EncodingFactory.Encoding.Write(encoder,StatusComments, nameof(StatusComments));
        // }
    }

    [ComplexObjectGenerator]
    public partial class WorkOrderStatus : ComplexObject
    {
        public string Actor;
        public DateTime Timestamp;
        public LocalizedText Comment;

        public WorkOrderStatus()
        {
            TypeIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=3004";
            BinaryEncodingIdNamespace = "nsu=http://www.unifiedautomation.com/DemoServer/;i=5011";
        }

        // public override void Decode(IDecoder decoder)
        // {
        //     base.Decode(decoder);
        //     decoder.Read(ref Actor, nameof(Actor));
        //     decoder.Read(ref Timestamp, nameof(Timestamp));
        //     decoder.Read(ref Comment, nameof(Comment));
        // }
        //
        // public override void Encode(IEncoder encoder)
        // {
        //     base.Encode(encoder);
        //     encoder.Write(Actor, nameof(Actor));
        //     encoder.Write(Timestamp, nameof(Timestamp));
        //     encoder.Write(Comment, nameof(Comment));
        // }
    }
}