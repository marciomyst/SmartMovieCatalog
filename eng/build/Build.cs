using System;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tooling;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tooling.ProcessTasks;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Test);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Mutation scope: domain, application, both")]
    readonly string MutationScope = "both";

    AbsolutePath SolutionFile => RootDirectory / "SmartMovieCatalog.slnx";
    AbsolutePath FrontendDirectory => RootDirectory / "frontend";
    AbsolutePath CoverageRunSettings => RootDirectory / "backend" / "tests" / "coverage.runsettings";
    AbsolutePath CoverageResultsDirectory => RootDirectory / "backend" / "tests" / "TestResults" / "coverage";
    AbsolutePath MutationScript => RootDirectory / "eng" / "scripts" / "test-mutation.ps1";
    AbsolutePath RunLocalScript => RootDirectory / "eng" / "scripts" / "run-local.ps1";

    Target Clean => _ => _
        .Executes(() =>
        {
            DotNetClean(s => s
                .SetProject(SolutionFile)
                .SetConfiguration(Configuration));

            var generatedDirectories = new[]
            {
                RootDirectory / "artifacts",
                RootDirectory / "TestResults",
                RootDirectory / "backend" / "tests" / "TestResults",
                RootDirectory / "frontend" / "dist",
                RootDirectory / "frontend" / ".angular" / "cache",
                RootDirectory / "frontend" / "obj"
            };

            foreach (var directory in generatedDirectories)
            {
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, recursive: true);
                }
            }
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNet("tool restore");
            DotNetRestore(s => s.SetProjectFile(SolutionFile));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(SolutionFile)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target TestBackend => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(SolutionFile)
                .SetConfiguration(Configuration)
                .EnableNoBuild());
        });

    Target TestFrontend => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            StartProcess("npm", "test -- --watch=false", FrontendDirectory)
                .AssertZeroExitCode();
        });

    Target Test => _ => _
        .DependsOn(TestBackend, TestFrontend);

    Target BuildFrontend => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            StartProcess("npm", "run build", FrontendDirectory)
                .AssertZeroExitCode();
        });

    Target Coverage => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(SolutionFile)
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .SetSettingsFile(CoverageRunSettings)
                .SetDataCollector("XPlat Code Coverage")
                .SetResultsDirectory(CoverageResultsDirectory));
        });

    Target Mutation => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            var scope = MutationScope.ToLowerInvariant();
            var allowedScopes = new[] { "domain", "application", "both" };
            if (!allowedScopes.Contains(scope))
            {
                throw new Exception($"Invalid MutationScope '{MutationScope}'. Use one of: domain, application, both.");
            }

            StartProcess(
                    "powershell",
                    $"-NoProfile -ExecutionPolicy Bypass -File \"{MutationScript}\" -Scope {scope}",
                    RootDirectory)
                .AssertZeroExitCode();
        });

    Target RunLocal => _ => _
        .Executes(() =>
        {
            StartProcess(
                    "powershell",
                    $"-NoProfile -ExecutionPolicy Bypass -File \"{RunLocalScript}\"",
                    RootDirectory)
                .AssertZeroExitCode();
        });

    Target BuildAll => _ => _
        .DependsOn(Compile, BuildFrontend);
}
