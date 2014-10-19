using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using VisualStudioCleaner.Common.Domain.Cleaners;
using VisualStudioCleaner.Common.Domain.Finders;

namespace VisualStudioCleaner.Cleaners.IOCleaners
{
    [Export( typeof( IDirectoryCleaner ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal sealed class DirectoryCleaner : IDirectoryCleaner
    {
        private readonly IDirectoryFinder _directoryFinder;

        [ImportingConstructor]
        public DirectoryCleaner( IDirectoryFinder directoryFinder )
        {
            _directoryFinder = directoryFinder;
        }

        public List<string> Clean( IEnumerable<string> searchPatterns )
        {
            List<string> directories = _directoryFinder.Find( searchPatterns );

            var deleted = new List<string>();
            foreach( string directory in directories )
            {
                DeleteDirectory( directory, deleted );
            }

            return deleted;
        }

        /// <summary>
        /// Ensures a directory can be deleted by removing ReadOnly
        /// flags from the files within the directory and the directory itself.
        /// </summary>
        private void DeleteDirectory( string directoryPath, List<string> deleted )
        {
            if( !Directory.Exists( directoryPath ) )
            {
                return;
            }

            string[] directories = Directory.GetDirectories( directoryPath );
            foreach( string path in directories )
            {
                DeleteDirectory( path, deleted );
            }

            RemoveReadOnlyFlags( directoryPath );

            Directory.Delete( directoryPath, true );
            deleted.Add( directoryPath );
        }

        private void RemoveReadOnlyFlags( string directoryPath )
        {
            var di = new DirectoryInfo( directoryPath )
            {
                Attributes = FileAttributes.Normal
            };

            FileInfo[] files = di.GetFiles();
            foreach( FileInfo file in files )
            {
                file.Attributes = FileAttributes.Normal;
            }
        }
    }
}
