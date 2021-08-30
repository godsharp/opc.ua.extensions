using Nuke.Common.CI.GitHubActions;

[GitHubActions(
    "continuous",
    GitHubActionsImage.WindowsLatest,
    GitHubActionsImage.UbuntuLatest,
    GitHubActionsImage.MacOsLatest,
    AutoGenerate = true,
    PublishArtifacts = false,
    // On = new[] { GitHubActionsTrigger.Push },
    OnPushBranches = new []{"main"},
    InvokedTargets = new []{nameof(Compile)},
    //ImportSecrets = new []{nameof(GhAccessToken)},
    CacheKeyFiles = new string[0]
)]
partial class Build
{
}