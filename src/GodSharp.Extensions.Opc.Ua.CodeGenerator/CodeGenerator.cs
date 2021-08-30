using System.IO;
using System.Text;

using static GodSharp.Extensions.Opc.Ua.CodeGenerator.CodeGeneratorTool;

namespace GodSharp.Extensions.Opc.Ua.CodeGenerator
{
    public class CodeGenerator
    {
        public string Build(CodeGeneratorMetadata metadata, bool toFile = true)
        {
            if (metadata == null) return null;

            StringBuilder builder = new();
            builder.AppendLine(string.Format(Header,"MSBuild", Path.GetFileName(metadata.SourceFile)));
            foreach (var item in metadata.Usings) builder.AppendLine(item);
            builder.AppendLine();
            builder.AppendLine($"namespace {metadata.Namespace}");
            builder.AppendLine("{");
            for (var i = 0; i < metadata.Types.Count; i++)
            {
                if (i > 0) builder.AppendLine();
                BuildType(builder, metadata.Types[i]);
            }
            builder.AppendLine("}");

            //File.Delete(metadata.OutputFile);
            if(toFile) File.WriteAllText(metadata.OutputFile, builder.ToString());
            return builder.ToString();
        }
    }
}
