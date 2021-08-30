using Nuke.Common.CI.GitHubActions;

[GitHubActions(
    "continuous",
    GitHubActionsImage.WindowsLatest,
    AutoGenerate = true,
    PublishArtifacts = true,
    // On = new[] { GitHubActionsTrigger.Push },
    OnPushBranches = new[] { "main" },
    InvokedTargets = new[] { nameof(Deploy) },
    ImportSecrets = new[] { nameof(GhAccessToken),nameof(NuGetApiKey),nameof(MyGetApiKey) },
    CacheKeyFiles = new string[0]
)]
[GitHubActions(
    "continuous.ci",
    GitHubActionsImage.UbuntuLatest,
    GitHubActionsImage.MacOsLatest,
    AutoGenerate = true,
    PublishArtifacts = false,
    // On = new[] { GitHubActionsTrigger.Push },
    OnPushBranches = new[] { "main" },
    InvokedTargets = new[] { nameof(Compile) },
    //ImportSecrets = new[] { nameof(GhAccessToken) },
    CacheKeyFiles = new string[0]
)]
partial class Build
{
}