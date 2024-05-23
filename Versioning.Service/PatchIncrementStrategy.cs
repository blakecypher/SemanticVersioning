namespace Versioning.Service;

public class PatchIncrementStrategy : IIncrementationStrategy
{
    /// <summary>
    /// Gets the incremented version by increasing the build number by 1.
    /// </summary>
    /// <param name="version">The current version.</param>
    /// <returns>The incremented version.</returns>
    public Task<Version> GetIncrementedVersion(Version version)
    {
        return Task.Run(() => new Version(version.Major, version.Minor, version.Build + 1));
    }
}