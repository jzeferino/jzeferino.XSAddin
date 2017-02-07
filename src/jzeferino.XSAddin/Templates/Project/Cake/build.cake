#addin "Cake.Xamarin&version=1.3.0.6"
#addin "Cake.AndroidAppManifest"

#tool "xunit.runner.console&version=2.1.0"
#tool "XamarinComponent&version=1.1.0.39"

var target = Argument("target", "Default");
var androidVersion = Argument<int?>("android_version", null);

var configuration = "Release";
var xamarinUsername = Argument("xamarin_username", "");
var XamarinPassword = Argument("xamarin_password", "");

// Define directories.
var solutionFile = GetFiles("./*.sln").First();

// Android and iOS.
var androidProject = GetFiles("./src/android/${SolutionName}.Droid/*.csproj").First();
var manifestFile = File("./src/android/${SolutionName}.Droid/Properties/AndroidManifest.xml");
var iosProjectRoot = "./src/ios/${SolutionName}.iOS";
var iOSProject = GetFiles(iosProjectRoot + "/*.csproj").First();

// Tests.
var allTestsProject = GetFiles("./test/**/*.csproj");
var allTestsDll = string.Format("./test/**/bin/{0}/*.Tests.dll", configuration);

// Output folder.
var artifactsDir = Directory("./artifacts");
var iOSOutputDirectory = "bin/";

// Xamarin Component.
var xamarinComponetPath = "./tools/XamarinComponent/tools/xamarin-component.exe";
var XamarinComponetEmail = xamarinUsername;
var XamarinComponetPassword = XamarinPassword;

// Android keystore information.
var keyStore = "./keystore/keystore.jks";
var keyStoreAlias = "";
var keyStorePassword = "";

// Build configuration
var isLocalBuild = BuildSystem.IsLocalBuild;

#region Clean

Task("Clean-Solution")
	.IsDependentOn("Prepare")
	.IsDependentOn("Clean-ArtifactsFolder")
	.IsDependentOn("Clean-iOSOutPutFolder")
	.Does(() => 
	{
		DotNetBuild(solutionFile, settings => settings
			.SetConfiguration(configuration)
			.WithTarget("Clean")
			.SetVerbosity(Verbosity.Minimal));
	});

Task("Clean-ArtifactsFolder")
	.Does(() => 
	{
		CleanDirectory(artifactsDir);
	});

Task("Clean-iOSOutputFolder")
	.Does(() => 
	{
		// There are some files in the bin directory after iOSBuild.
		CleanDirectories(iosProjectRoot + "/bin");
	});	

#endregion

Task("Restore-XamarinComponents")
	.Does(() => 
	{
		RestoreComponents(solutionFile, new XamarinComponentRestoreSettings {
			ToolPath = xamarinComponetPath,
			Email = XamarinComponetEmail,
			Password = XamarinComponetPassword
		});
	});

Task("Restore-Packages")
	.Does(() => 
	{
		NuGetRestore(solutionFile);
	});

#region Unit Test

Task("Build-Tests")
	.IsDependentOn("Prepare-Build")
    .Does(() =>
	{
		foreach(var csproj in allTestsProject)
		{		
			DotNetBuild(csproj.FullPath, settings => settings
				.SetConfiguration(configuration)
				.WithTarget("Rebuild")
				.SetVerbosity(Verbosity.Minimal));
		}		
    });

Task("Run-Tests")
	// Allows the build process to continue even if there Tests aren't passing.
	.ContinueOnError()
	.IsDependentOn("Build-Tests")
    .Does(() =>
	{
		foreach(var testDll in GetFiles(allTestsDll))
		{
			XUnit2(testDll.FullPath, new XUnit2Settings {
				XmlReport = true,
				OutputDirectory = artifactsDir
			});
		}
    });

#endregion

Task("Prepare-Build")
	.IsDependentOn("Clean-Solution")
    .IsDependentOn("Restore-Packages")
	.IsDependentOn("Restore-XamarinComponents")	
    .Does (() => {});

Task("Build-Android")
	.IsDependentOn("Prepare-Build")
    .Does(() =>
	{ 		
		 DotNetBuild(androidProject, settings =>
        	settings.SetConfiguration(configuration)         
            .WithProperty("DebugSymbols", "false")
            .WithProperty("TreatWarningsAsErrors", "false")
			.SetVerbosity(Verbosity.Minimal));        
    });

Task("UpdateAndroidManifest")
    .WithCriteria(() => isLocalBuild)
    .Does (() =>
{
	var manifest = DeserializeAppManifest(manifestFile);
	
	manifest.VersionCode = androidVersion.HasValue ? androidVersion.Value : ++manifest.VersionCode;	

    SerializeAppManifest(manifestFile, manifest);
});

Task("Release-Android")
	.IsDependentOn("UpdateAndroidManifest")
	.IsDependentOn("Prepare-Build")
    .Does(() =>
	{ 		
		 DotNetBuild(androidProject, settings =>
        	settings.SetConfiguration(configuration)
            .WithTarget("SignAndroidPackage")
            .WithProperty("AndroidKeyStore", "true")
            .WithProperty("AndroidSigningStorePass", keyStorePassword)
            .WithProperty("AndroidSigningKeyStore", keyStore)
            .WithProperty("AndroidSigningKeyAlias", keyStoreAlias)
            .WithProperty("AndroidSigningKeyPass", keyStorePassword)
            .WithProperty("DebugSymbols", "true")
            .WithProperty("TreatWarningsAsErrors", "false")
			.SetVerbosity(Verbosity.Verbose));

            // Search for the signed apk and copy it to distribution directory.
            var searchPattern = androidProject.GetDirectory() + "/**/*-Signed.apk";

            var apkFilePath = GetFiles (searchPattern)
			                .OrderByDescending (f => new FileInfo (f.FullPath).LastWriteTimeUtc)
			                .FirstOrDefault(); 	

          	// Copy the APK to distribution directory.		
			CopyFile(apkFilePath, artifactsDir + File(apkFilePath.GetFilename().ToString()));
    });

Task("Build-iOS")
	.IsDependentOn("Prepare-Build")
    .Does (() =>
{
    DotNetBuild(iOSProject, settings =>
		settings.SetConfiguration(configuration)
		.WithTarget("Build")
		.WithProperty("Platform", "iPhone")
		.WithProperty("TreatWarningsAsErrors", "false")
		.WithProperty("OutputPath", iOSOutputDirectory)
		.SetVerbosity(Verbosity.Minimal));
});

Task("Release-iOS")
	.IsDependentOn("Prepare-Build")
    .Does (() =>
{
    DotNetBuild(iOSProject, settings =>
		settings.SetConfiguration(configuration)
		.WithTarget("Build")
		.WithProperty("Platform", "iPhone")
		.WithProperty("IpaPackageDir", iOSOutputDirectory)
		.WithProperty("TreatWarningsAsErrors", "false")
		.WithProperty("OutputPath", iOSOutputDirectory)
		.WithProperty("BuildIpa", "true")
		.WithProperty("ArchiveOnBuild", "true")
		.SetVerbosity(Verbosity.Minimal));

		// Search for the signed ipa and copy it to distribution directory.
		var searchPattern = iOSProject.GetDirectory() + "/**/*.ipa";
		var ipaFilePath = GetFiles (searchPattern).FirstOrDefault(); 
		CopyFile(ipaFilePath, artifactsDir + File(ipaFilePath.GetFilename().ToString()));
});

Task("Prepare")
    .Does (() =>
{
    Information("Android version: " + androidVersion);
	Information("Target: " + target);
});

Task("Build-All")
    .IsDependentOn("Build-Android")
    .IsDependentOn("Build-iOS")
    .Does (() => {});

Task("Release-All")
    .IsDependentOn("Release-Android")
    .IsDependentOn("Release-iOS")
    .Does (() => {});

Task("Default")
	.IsDependentOn("Build-All")
	.IsDependentOn("Run-Tests");

RunTarget(target);

