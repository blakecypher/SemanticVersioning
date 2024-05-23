using Microsoft.Extensions.DependencyInjection;
using Versioning.Model;
using Versioning.Persistence;

namespace Versioning.Service;

public class VersioningService(
    IPersistenceService persistenceService,
    IServiceProvider serviceProvider)
    : IVersioningService
{
    /// <summary>
    /// Gets the appropriate incrementation strategy based on the provided version type.
    /// (Strategy Pattern)
    /// </summary>
    /// <param name="versionType">The type of version to determine the incrementation strategy for.</param>
    /// <returns>The corresponding incrementation strategy wrapped in a Task.</returns>
    public Task<IIncrementationStrategy> GetIncrementationStrategy(VersionType versionType)
    {
        return versionType switch
        {
            VersionType.Major => Task.FromResult(serviceProvider.GetRequiredService<MajorIncrementStrategy>() as IIncrementationStrategy),
            VersionType.Minor => Task.FromResult(serviceProvider.GetRequiredService<MinorIncrementStrategy>() as IIncrementationStrategy),
            VersionType.Patch => Task.FromResult(serviceProvider.GetRequiredService<PatchIncrementStrategy>() as IIncrementationStrategy),
            _ => throw new ArgumentOutOfRangeException(nameof(versionType), versionType, "Version type not supported")
        };
    }
    
    /// <summary>
    /// Retrieves the project details asynchronously.
    /// </summary>
    /// <returns>The project details wrapped in a Task.</returns>
    public Task<ProjectDetails?> GetProjectDetails()
    {
        return persistenceService.GetProjectDetails();
    }

    /// <summary>
    /// Gets the incremented version based on the provided strategy and initial version.
    /// </summary>
    /// <param name="strategy">Task containing the incrementation strategy to use.</param>
    /// <param name="version">The initial version to be incremented.</param>
    /// <returns>The incremented version.</returns>
    public async Task<Version> GetIncrementedVersion(Task<IIncrementationStrategy> strategy, Version version)
    {
        IIncrementationStrategy selectedStrategy = await strategy;
        return selectedStrategy.GetIncrementedVersion(version).Result;
    }

    /// <summary>
    /// Saves new project details and returns a new ProjectDetails object.
    /// </summary>
    /// <param name="projectDetails">The ProjectDetails object to be saved.</param>
    /// <returns>A new ProjectDetails object with updated information.</returns>
    public async Task<ProjectDetails> SaveNewProjectDetails(ProjectDetails projectDetails)
    {
        // Save project details using the persistence service
        await persistenceService.SaveProjectDetails(projectDetails);

        // Create and return a new ProjectDetails object based on the input
        return new ProjectDetails(projectDetails.ToString(), projectDetails.Patch);
    }
}