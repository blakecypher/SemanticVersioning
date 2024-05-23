using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Versioning.Model;

namespace Versioning.Persistence;

public class PersistenceService(IConfiguration configuration) : IPersistenceService
{
    private readonly string? projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
    private readonly string? jsonContents = File.ReadAllText(configuration["ProjectDetails"] ?? string.Empty);
    
    /// <summary>
    /// Retrieves project details asynchronously.
    /// </summary>
    /// <returns>The project details.</returns>
    public Task<ProjectDetails?> GetProjectDetails()
    {
        var semanticVersion = JsonSerializer.Deserialize<ProjectDetails>(jsonContents ?? string.Empty);

        return Task.FromResult(semanticVersion);
    }

    /// <summary>
    /// Saves the project details to a JSON file.
    /// </summary>
    /// <param name="projectDetails">The project details to be saved.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task SaveProjectDetails(ProjectDetails projectDetails)
    {
        // Check if the project root is specified
        if (projectDirectory != null)
        {
            // Construct the JSON file path
            var jsonFilePath = Path.Combine(projectDirectory, configuration["ProjectDetails"] ?? string.Empty);
           
            // Save the JSON file
            File.WriteAllText(jsonFilePath, JsonSerializer.Serialize(projectDetails, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }

        return Task.CompletedTask;
    }
}