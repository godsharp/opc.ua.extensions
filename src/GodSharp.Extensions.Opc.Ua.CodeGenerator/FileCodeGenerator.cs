using System.IO;

using static GodSharp.Extensions.Opc.Ua.CodeGenerator.CodeGeneratorTool;

namespace GodSharp.Extensions.Opc.Ua.CodeGenerator
{
    public class FileCodeGenerator
    {
        public void Execute(string path)
        {
            var files = Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories);
            var generator = new CodeGenerator();
            foreach (var file in files)
            {
                if (file.Contains("\\obj\\")) continue;
                if (file.EndsWith(Header)) continue;
                generator.Build(ParseFile(file));
            }
        }

    }
}
