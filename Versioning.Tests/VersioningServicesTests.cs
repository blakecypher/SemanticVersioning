using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Versioning.Model;
using Versioning.Persistence;
using Versioning.Service;
using Xunit;

namespace VersioningApp.Tests
{
    public class VersioningServiceTests
    {
        private readonly Mock<IPersistenceService> _mockPersistenceService;
        private readonly VersioningService _versioningService;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor for VersioningServiceTests class.
        /// Initializes the mock IPersistenceService and sets up services for Major, Minor, and Patch increment strategies.
        /// </summary>
        public VersioningServiceTests()
        {
            // Initialize mock IPersistenceService
            _mockPersistenceService = new Mock<IPersistenceService>();

            // Set up services for Major, Minor, and Patch increment strategies
            var services = new ServiceCollection();
            services.AddScoped(s => _mockPersistenceService.Object);
            services.AddTransient<MajorIncrementStrategy>();
            services.AddTransient<MinorIncrementStrategy>();
            services.AddTransient<PatchIncrementStrategy>();
            
            // Initialize VersioningService with mock IPersistenceService and service provider
            _serviceProvider = services.BuildServiceProvider();
            _versioningService = new VersioningService(_mockPersistenceService.Object, _serviceProvider);

        }

        [Theory]
        [InlineData(VersionType.Major, typeof(MajorIncrementStrategy))]
        [InlineData(VersionType.Minor, typeof(MinorIncrementStrategy))]
        [InlineData(VersionType.Patch, typeof(PatchIncrementStrategy))]
        public async Task GetIncrementationStrategy_ShouldReturnCorrectStrategy(VersionType versionType, Type expectedStrategyType)
        {
            // Act
            var strategy = await _versioningService.GetIncrementationStrategy(versionType);

            // Assert
            Assert.NotNull(strategy);
            Assert.IsType(expectedStrategyType, strategy);
        }

        [Fact]
        public async Task GetIncrementedVersion_ShouldReturnIncrementedVersion()
        {
            // Arrange
            var currentVersion = new Version(1, 0, 0);
            var strategy = await _versioningService.GetIncrementationStrategy(VersionType.Major);

            // Act
            var incrementedVersion = await _versioningService.GetIncrementedVersion(Task.FromResult(strategy), currentVersion);

            // Assert
            Assert.Equal(new Version(2, 0, 0), incrementedVersion);
        }

        [Fact]
        public async Task GetProjectDetails_ShouldReturnProjectDetails()
        {
            // Arrange
            var expectedProjectDetails = new ProjectDetails("1.0.0", new Patch("PatchName", "Directory", "1",
                ["script1.sql", "script2.sql"]));
            _mockPersistenceService.Setup(p => p.GetProjectDetails()).ReturnsAsync(expectedProjectDetails);

            // Act
            var projectDetails = await _versioningService.GetProjectDetails();

            // Assert
            Assert.NotNull(projectDetails);
            Assert.Equal(expectedProjectDetails.Version, projectDetails.Version);
            Assert.Equal(expectedProjectDetails.Patch.Name, projectDetails.Patch.Name);
        }

        [Fact]
        public async Task SaveNewProjectDetails_ShouldSaveAndReturnProjectDetails()
        {
            // Arrange
            var projectDetails = new ProjectDetails("1.0.1", new Patch("PatchName", "Directory", "1",
                ["script1.sql", "script2.sql"]));
            
            _mockPersistenceService.Setup(p => p.SaveProjectDetails(It.IsAny<ProjectDetails>())).Returns(Task.CompletedTask);

            // Act
            var savedProjectDetails = await _versioningService.SaveNewProjectDetails(projectDetails);

            // Assert
            _mockPersistenceService.Verify(p => p.SaveProjectDetails(It.IsAny<ProjectDetails>()), Times.Once);
            Assert.Equal(projectDetails.Version, savedProjectDetails.Version);
            Assert.Equal(projectDetails.Patch.Name, savedProjectDetails.Patch.Name);
        }
    }
}
