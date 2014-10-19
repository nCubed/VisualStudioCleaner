using VisualStudioCleaner.Common.Domain;

namespace DependencyResolution.UnitTests
{
    internal class MockRootDirectory : IRootDirectory
    {
        public string RootDirectory { get; private set; }

        public MockRootDirectory( string rootDirectory )
        {
            RootDirectory = rootDirectory;
        }
    }
}