using System;
using GlobExpressions;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Pack);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Api key to push packages to nuget.org.")] 
    [Secret]
    string NuGetApiKey;
    [Parameter("Api key to push packages to myget.org.")]
    [Secret]
    string MyGetApiKey;
    [Parameter("Github personal access token.")]
    [Secret]
    string GhAccessToken;

    [Solution] readonly Solution Solution;
    //[GitRepository] readonly GitRepository GitRepository;
    //[GitVersion(Framework = "net5.0", NoFetch = true)] readonly GitVersion GitVersion;

    [CI] readonly AzurePipelines AzurePipelines;
    //[CI] readonly GitHubActions GitHubActions;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath OutputDirectory => RootDirectory / "output";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    HostType HostType = HostType.None;

    protected override void OnBuildInitialized()
    {
        base.OnBuildInitialized();
        NuGetApiKey ??= Environment.GetEnvironmentVariable(nameof(NuGetApiKey));
        MyGetApiKey ??= Environment.GetEnvironmentVariable(nameof(MyGetApiKey));
        GhAccessToken ??= Environment.GetEnvironmentVariable(nameof(GhAccessToken));
        Enum.TryParse(Host.Instance.GetType().Name, true, out HostType);
    }
    
    Target Clean => _ => _
        .Description("Clean Solution")
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            OutputDirectory.GlobDirectories("*").ForEach(DeleteDirectory);
            DeleteDirectory(OutputDirectory);
            DeleteDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Description("Restore Solution")
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .Description("Compile Solution")
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target Pack => _ => _
        .Description("Package Project")
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution)
                .SetConfiguration(Configuration)
                .SetDescription("Opc.Ua produced by GodSharp")
                .SetPackageTags("OpcUa", "Opc", "Ua", "GodSharp")
                .SetOutputDirectory(ArtifactsDirectory)
                .EnableNoRestore()
            );
        });

    Target Artifacts => _ => _
        .DependsOn(Pack)
        .OnlyWhenStatic(() => IsServerBuild, () => Configuration.Equals(Configuration.Release))
        .Description("Upload Artifacts")
        .Executes(() =>
        {
            if (HostType == HostType.AzurePipelines)
            {
                Logger.Info("Upload artifacts to azure...");
                AzurePipelines
                    .UploadArtifacts("artifacts", "artifacts", ArtifactsDirectory);
                Logger.Info("Upload artifacts to azure finished."); 
            }
        });

    Target Push => _ => _
        .Description("Push NuGet Package")
        .DependsOn(Pack)
        .OnlyWhenStatic(() => IsServerBuild, () => Configuration.Equals(Configuration.Release))
        //.Requires(() => NuGetApiKey)
        //.Requires(() => MyGetApiKey)
        .Executes(() =>
        {
            Logger.Info("Push nuget package to server...");
            GlobFiles(ArtifactsDirectory, "**/*.nupkg").ForEach(Nuget);
            Logger.Info("Push nuget package to server finished.");
        });

    Target Deploy => _ => _
        .Description("Deploy")
        .DependsOn(Push, Artifacts)
        .Executes(() =>
        {
            Logger.Info("Deployed");
        });
    
    void Nuget(string x)
    {
        if (NuGetApiKey != null) 
            Nuget(x, "https://api.nuget.org/v3/index.json", NuGetApiKey);

        if (MyGetApiKey != null) 
            Nuget(x, "https://www.myget.org/F/godsharp/api/v2/package", MyGetApiKey);

        if (HostType == HostType.GitHubActions && GhAccessToken != null)
            Nuget(x, "https://nuget.pkg.github.com/godsharp/index.json", GhAccessToken);
    }

    void Nuget(string x, string source, string key) =>
        DotNetNuGetPush(s => s
            .SetTargetPath(x)
            .SetSource(source)
            .SetApiKey(key));
}