using Nuke.Common.CI.AzurePipelines;

[AzurePipelines(
    null,
    AzurePipelinesImage.WindowsLatest,
    AutoGenerate = true,
    TriggerBranchesInclude = new[] {"main"},
    InvokedTargets = new[] {nameof(Deploy)},
    NonEntryTargets = new[] { nameof(Clean), nameof(Restore), nameof(Compile), nameof(Pack),nameof(Push),nameof(Artifacts)},
    ImportSecrets = new[] {nameof(NuGetApiKey),nameof(MyGetApiKey)},
    CacheKeyFiles = new string[0]
)]
partial class Build
{
}