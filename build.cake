#addin "Cake.Xamarin&version=1.3.0.14"

var target = Argument("target", "Default");
var configuration = "Release";

// Define directories.
var solutionFile = GetFiles("./*.sln").First();

// Addin project.
var binaryPath = File("./src/jzeferino.XSAddin/bin/" + configuration + "jzeferino.XSAddin.dll");

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
			.SetVerbosity(Verbosity.Verbose));
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
    .Does(() =>
	{ 		
		 DotNetBuild(solutionFile, settings =>
        	settings.SetConfiguration(configuration)         
            .WithProperty("TreatWarningsAsErrors", "false")
			.SetVerbosity(Verbosity.Quiet));        
    });

Task("Pack")
	.IsDependentOn("Build")
    .WithCriteria(() => isLocalBuild)
	.Does(() => 
	{
		MDToolSetup.Pack(binaryPath, artifactsDir);
	});

Task("Default")
	.IsDependentOn("Pack");

RunTarget(target);

