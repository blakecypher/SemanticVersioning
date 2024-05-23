namespace Versioning.Service;

public interface IIncrementationStrategy
{
    Task<Version> GetIncrementedVersion(Version version);
}