using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Versioning.Model;
using Versioning.Persistence;
using Versioning.Service;

// Check ares length for correct number of arguments
if (args.Length is 0 or > 1)
{
    throw new ArgumentException("Specify a single version type to increment as: major, minor, patch");
}

// Get project details
var host = CreateHostBuilder(args).Build();
var versioningService = host.Services.GetRequiredService<IVersioningService>();
var projectDetails = versioningService.GetProjectDetails().Result;
var version = new Version(projectDetails?.Version ?? string.Empty);


// Get incrementation strategy
using var strategy = args[0] switch
{
    "major" => Task.FromResult(await versioningService.GetIncrementationStrategy(VersionType.Major)),
    "minor" => Task.FromResult(await versioningService.GetIncrementationStrategy(VersionType.Minor)),
    "patch" => Task.FromResult(await versioningService.GetIncrementationStrategy(VersionType.Patch)),
    _ => throw new ArgumentException("Specify a single version type to increment as: major, minor, patch")
};

// Get incremented version
var incrementedVersion = await versioningService.GetIncrementedVersion(strategy, version);
projectDetails.Version = incrementedVersion.ToString();

// Save project details
await versioningService.SaveNewProjectDetails(projectDetails);

return;


// Helpers
static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((_, config) =>
        {
            config
                .AddJsonFile("ProjectDetails.json");
            config.AddJsonFile("appsettings.json");
        })
        .ConfigureServices((_, services) =>
        {
            services.AddTransient<IPersistenceService, PersistenceService>();
            services.AddTransient<IVersioningService, VersioningService>();
            services.AddTransient<MajorIncrementStrategy>();
            services.AddTransient<MinorIncrementStrategy>();
            services.AddTransient<PatchIncrementStrategy>();
        });
}