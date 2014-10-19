using System.Collections.Generic;

namespace VisualStudioCleaner.Workers
{
    public interface IVSCleanerResults
    {
        IEnumerable<string> DirectoriesDeleted { get; }
        IEnumerable<string> FilesDeleted { get; }
        IEnumerable<string> SouceControlFilesCleaned { get; }
    }
}