using System.Collections.Generic;

namespace VisualStudioCleaner.Workers
{
    internal sealed class VSCleanerResults : IVSCleanerResults
    {
        internal List<string> Directories;
        internal List<string> Files;
        internal List<string> SouceControl;

        public IEnumerable<string> DirectoriesDeleted { get { return Directories; } }
        public IEnumerable<string> FilesDeleted { get { return Files; } }
        public IEnumerable<string> SouceControlFilesCleaned { get { return SouceControl; } }

        public VSCleanerResults()
        {
            Directories = new List<string>();
            Files = new List<string>();
            SouceControl = new List<string>();
        }
    }
}