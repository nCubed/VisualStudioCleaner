using System.IO;
using VisualStudioCleaner.Common.Domain;

namespace VisualStudioCleaner.UI.CommandLine
{
    internal class VSCleanerRootDirectory : IRootDirectory
    {
        public string RootDirectory { get; private set; }

        public VSCleanerRootDirectory( string rootDirectory )
        {
            RootDirectory = rootDirectory;

            if( !Directory.Exists( RootDirectory ) )
            {
                string msg = string.Format( "Root Directory does not exist: {0}.", RootDirectory );
                throw new DirectoryNotFoundException( msg );
            }
        }
    }
}