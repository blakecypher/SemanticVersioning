using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Versioning.Model;

namespace Versioning.Persistence;

public class PersistenceService(IConfiguration configuration) : IPersistenceService
{
    private readonly string? projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
    private readonly string? jsonContents = File.ReadAllText(configuration["ProjectDetails"] ?? string.Empty);
    
    
    public Task<ProjectDetails?> GetProjectDetails()
    {
        var semanticVersion = JsonSerializer.Deserialize<ProjectDetails>(jsonContents ?? string.Empty);

        return Task.FromResult(semanticVersion);
    }

    public Task SaveProjectDetails(ProjectDetails projectDetails)
    {
        if (projectDirectory != null)
        {
            var jsonFilePath = Path.Combine(projectDirectory, configuration["ProjectDetails"] ?? string.Empty);
            File.WriteAllText(jsonFilePath, JsonSerializer.Serialize(projectDetails, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }

        return Task.CompletedTask;
    }
}