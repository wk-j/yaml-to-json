#addin "wk.StartProcess"
#addin "wk.ProjectParser"

using PS = StartProcess.Processor;
using ProjectParser;

var project = "src/Yaml2Json/Yaml2Json.fsproj";
var info = Parser.Parse(project);
var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
var currentDir = new System.IO.DirectoryInfo(".").FullName;

Task("Pack").Does(() => {
    CleanDirectory("publish");
    DotNetCorePack(project, new DotNetCorePackSettings {
        OutputDirectory = "publish"
    });
});

Task("Publish-Nuget")
    .IsDependentOn("Pack")
    .Does(() => {
        var npi = EnvironmentVariable("npi");
        var nupkg = new DirectoryInfo("publish").GetFiles("*.nupkg").LastOrDefault();
        var package = nupkg.FullName;
        NuGetPush(package, new NuGetPushSettings {
            Source = "https://www.nuget.org/api/v2/package",
            ApiKey = npi
        });
});

Task("Install")
    .IsDependentOn("Pack")
    .Does(() => {
        Information(info.Version);
        PS.StartProcess($"dotnet tool uninstall -g wk.Yaml2Json");
        PS.StartProcess($"dotnet tool install -g wk.Yaml2Json --source-feed {currentDir}/publish --version {info.Version}");
    });

var target = Argument("target", "Pack");
RunTarget(target);