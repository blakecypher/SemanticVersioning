namespace Versioning.Service;

public class MajorIncrementStrategy : IIncrementationStrategy
{
    /// <summary>
    /// Gets the incremented version by increasing the major version number by 1.
    /// </summary>
    /// <param name="version">The current version.</param>
    /// <returns>The incremented version.</returns>
    public Task<Version> GetIncrementedVersion(Version version)
    {
        return Task.Run(() => new Version(version.Major + 1, 0, 0));
    }
}