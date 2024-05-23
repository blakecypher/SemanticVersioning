namespace Versioning.Service;

public class MinorIncrementStrategy : IIncrementationStrategy
{
    /// <summary>
    /// Increments the minor version number of the given version.
    /// </summary>
    /// <param name="version">The version to increment.</param>
    /// <returns>The incremented version.</returns>
    public Task<Version> GetIncrementedVersion(Version version)
    {
        return Task.Run(() => version = new Version(version.Major, version.Minor + 1, 0));
    }
}