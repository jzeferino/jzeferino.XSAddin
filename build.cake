#addin "Cake.Xamarin&version=1.3.0.14"

var target = Argument("target", "Default");
var configuration = "Release";

// Define directories.
var solutionFile = GetFiles("./*.sln").First();

// Addin project.
var binaryPath = System.IO.Path.Combine("./src/jzeferino.XSAddin/bin", configuration, "jzeferino.XSAddin.dll");

// Output folder.
var artifactsDir = Directory("./artifacts");

// Build configuration
var isLocalBuild = BuildSystem.IsLocalBuild;

Task("Clean-Solution")
	.IsDependentOn("Clean-Folders")
	.Does(() => 
	{
		DotNetBuild(solutionFile, settings => settings
			.SetConfiguration(configuration)
			.WithTarget("Clean")
			.SetVerbosity(Verbosity.Quiet));
	});

Task("Clean-Folders")
	.Does(() => 
	{
		CleanDirectory(artifactsDir);
		CleanDirectories ("./**/bin");
		CleanDirectories ("./**/obj");
	});

Task("Restore-Packages")
	.Does(() => 
	{
		NuGetRestore(solutionFile);
	});

Task("Build")
	.IsDependentOn("Clean-Solution")
    .IsDependentOn("Restore-Packages")
    .WithCriteria(() => isLocalBuild) // Temporary solution to bypass AppVeyor Error
    .Does(() =>
	{ 		
		 DotNetBuild(solutionFile, settings =>
        	settings.SetConfiguration(configuration)   
			.WithTarget("Build")
            .WithProperty("TreatWarningsAsErrors", "false")
			.SetVerbosity(Verbosity.Quiet));        
    });

Task("Pack")
	.IsDependentOn("Build")
    .WithCriteria(() => isLocalBuild)
    .WithCriteria(() => HasMdTool())
	.Does(() => 
	{
		MDToolSetup.Pack(binaryPath, artifactsDir);
	});

private bool HasMdTool()
{
	var mdToolPath = @"/Applications/Xamarin Studio.app/Contents/Resources/lib/monodevelop/bin/mdtool.exe";

	if (FileExists(mdToolPath)) {
		Information("mdtool exists");

		Context.Tools.RegisterFile(mdToolPath);
		return true;
	}
	return false;
}

Task("Default")
	.IsDependentOn("Pack");

RunTarget(target);

