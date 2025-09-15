// Install tools.
#tool dotnet:?package=coveralls.net&version=4.0.1

// Install addins.
#addin nuget:?package=Cake.Coveralls&version=4.0.0

var publishingError = false;

Task("Upload-Coverage-Result-coveralls")
    .Does(() =>
{
    var coverageFile = "C:\\temp\\coverage.xml";

    CoverallsNet(new FilePath(coverageFile), CoverallsNetReportType.OpenCover, new CoverallsNetSettings()
    {
        RepoToken = "My Coveralls Token",
        UseRelativePaths = true
    });
}).OnError(exception =>
{
    Information(exception.Message);
    Information($"Failed to upload coverage result to Coveralls, but continuing with next Task...");
    publishingError = true;
});



Task("Default")
	.IsDependentOn("Upload-Coverage-Result-coveralls")
    .Finally(() =>
{
    if (publishingError)
        Warning("At least one exception occurred when executing non-essential tasks. These exceptions were ignored in order to allow the build script to complete.");
    else
        Information("Build script completed successfully.");
});

RunTarget("Default");
