using VisualStudioCleaner.Common;

namespace VisualStudioCleaner.Workers
{
    public interface IVSCleaner
    {
        void SetUp( string directorySearchPatterns, string fileExtensions, VSCleanerOptions options );
        IVSCleanerResults DoWork();
    }
}