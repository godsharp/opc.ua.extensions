using GodSharp.Extensions.Opc.Ua.Types;

using System.Collections.Generic;

namespace GodSharp.Extensions.Opc.Ua.CodeGenerator
{
    public class CodeGeneratorMetadata
    {
        public string SourceFile { get; set; }
        public List<string> Usings { get; set; }
        public string Namespace { get; set; }
        public List<CodeGeneratorMetadataType> Types { get; set; }
        public string OutputFile { get; set; }
        public CodeGeneratorMetadata()
        {
            Usings = new List<string>();
            Types = new List<CodeGeneratorMetadataType>();
        }
    }

    public class CodeGeneratorMetadataType
    {
        public string Accessibility { get; set; }
        public string ClassName { get; set; }
        public ComplexObjectType ObjectType { get; set; }
        public EncodingMethodType MethodType { get; set; }
        public List<FieldMetadata> Fields { get; set; }

        public CodeGeneratorMetadataType()
        {
            Fields = new List<FieldMetadata>();
        }
    }
}
