using Versioning.Model;

namespace Versioning.Service;

public interface IVersioningService
{
    Task<IIncrementationStrategy> GetIncrementationStrategy(VersionType versionType);
    Task<ProjectDetails?> GetProjectDetails();
    Task<Version> GetIncrementedVersion(Task<IIncrementationStrategy> strategy, Version version);
    Task<ProjectDetails> SaveNewProjectDetails(ProjectDetails projectDetails);
}