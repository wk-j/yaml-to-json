#addin "wk.StartProcess"

using PS = StartProcess.Processor;

Task("Pack").Does(() => {
    CleanDirectory("publish");
    DotNetCorePack("src/Yaml2Json", new DotNetCorePackSettings {
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
        PS.StartProcess("rm /Users/wk/.dotnet/tools/wk-y2j");
        PS.StartProcess("dotnet install tool -g wk.Yaml2Json --source ./publish");
    });

var target = Argument("target", "Pack");
RunTarget(target);