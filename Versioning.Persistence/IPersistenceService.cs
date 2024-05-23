using Versioning.Model;

namespace Versioning.Persistence;

public interface IPersistenceService
{
    Task<ProjectDetails?> GetProjectDetails();
    Task SaveProjectDetails(ProjectDetails projectDetails);
}