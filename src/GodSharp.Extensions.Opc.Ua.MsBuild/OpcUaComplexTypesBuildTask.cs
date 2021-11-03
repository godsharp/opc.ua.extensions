using Microsoft.Build.Framework;

using System;
using GodSharp.Extensions.Opc.Ua.CodeGenerator;

namespace GodSharp.Extensions.Opc.Ua.MsBuild
{
    public class OpcUaComplexTypesBuildTask : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string ProjectPath { get; set; }

        public override bool Execute()
        {
            try
            {
                //System.Diagnostics.Debugger.Launch();
                new FileCodeGenerator().Execute(ProjectPath);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogMessageFromText(ex.Message, MessageImportance.High);
                Log.LogError(ex.Message);
                return false;
            }
        }
    }
}
