namespace Versioning.Model;

public class ProjectDetails(string? version, Patch? patch)
{
    public string? Version { get; set; } = version;
    public Patch? Patch { get; init; } = patch;
}

public class Patch(string name, string directory, string ordinal, List<string> scripts)
{
    public string Name { get; set; } = name;
    public string Directory { get; set; } = directory;
    public string Ordinal { get; set; } = ordinal;
    public List<string> Scripts { get; set; } = scripts;
}