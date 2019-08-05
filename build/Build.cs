using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.PushGhPages);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    [Parameter] string GitHub_PAT;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "nuketests";
    AbsolutePath OutputDirectory => RootDirectory / "output";

    AbsolutePath PublishDirectory => RootDirectory / "../publish";

    AbsolutePath DistributionDirectory => PublishDirectory / @"miNationalrot" / @"dist";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            //TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(TestsDirectory);
            EnsureCleanDirectory(PublishDirectory);
            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s=>s.SetOutput(TestsDirectory));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.GetNormalizedAssemblyVersion())
                .SetFileVersion(GitVersion.GetNormalizedFileVersion())
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target Publish => _ => _
        .DependsOn(Clean)
        .DependsOn(Test)
        .Executes(() =>
        {
            DotNetPublish( s=>s
                .SetConfiguration("Release")
                .SetOutput(PublishDirectory)
                //.SetAssemblyVersion(GitVersion.GetNormalizedAssemblyVersion())
                .SetAssemblyVersion(GitVersion.FullSemVer.Replace("+","."))
                .SetFileVersion(GitVersion.GetNormalizedFileVersion())
                .SetInformationalVersion(GitVersion.InformationalVersion)
                //.SetNoBuild(true)
            );
        });


    Target PushGhPages => _ => _
        .DependsOn(Publish)
        .Executes(() =>
        {
            if (GitHub_PAT == null)
            {
                GitTasks.Git("init", DistributionDirectory);
                GitTasks.Git("checkout -b gh-pages", DistributionDirectory);
                GitTasks.Git("add -A", DistributionDirectory);
                GitTasks.Git($"commit -m \"commit ver {GitVersion.FullSemVer}\"", DistributionDirectory);
                GitTasks.Git("push -f  https://github.com/minationalrot/miNationalrot.git gh-pages", DistributionDirectory);
            }
            else
            {
                //GitTasks.Git("config --", DistributionDirectory);
                GitTasks.Git("init", DistributionDirectory);

                GitTasks.Git("checkout -b gh-pages", DistributionDirectory);
                GitTasks.Git("add -A", DistributionDirectory);
                GitTasks.Git($"commit -m \"commit ver {GitVersion.FullSemVer}\"", DistributionDirectory);
                GitTasks.Git($"push -f  https://{GitHub_PAT}@github.com/minationalrot/miNationalrot.git gh-pages", DistributionDirectory);

            }
            //GitRepository.

            //echo % PAT %
            //    echo % FL %
            //    git config--global user.name "$NAME"
            //git config --global user.email "$EMAIL"
            //cd $(build.artifactstagingdirectory) / minationalrot / dist
            //git init
            //git checkout -b gh - pages
            //git add -A
            //git commit -m "Create build $(Build.BuildId)"
            //rem git push - f https://$PAT@github.com/fernandreu/blazor-pages.git gh-pages
            //git push -f https://%PAT%@github.com/minationalrot/miNationalrot.git gh-pages
            //displayName: 'Publish to GitHub Pages'
            //env:
            //PAT: $(github_pat2)
            //    FL: $(Flag)
            //    NAME: RemoOser
            //EMAIL: remo.oser @osisa.com


            //DotNetBuild(s => s
            //    .SetProjectFile(Solution)
            //    .SetConfiguration(Configuration)
            //    .SetAssemblyVersion(GitVersion.GetNormalizedAssemblyVersion())
            //    .SetFileVersion(GitVersion.GetNormalizedFileVersion())
            //    .SetInformationalVersion(GitVersion.InformationalVersion)
            //    .EnableNoRestore());
        });

}
