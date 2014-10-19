using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using VisualStudioCleaner.Common.Domain.Cleaners;
using VisualStudioCleaner.Common.Domain.Finders;

namespace VisualStudioCleaner.Cleaners.IOCleaners
{
    [Export( typeof( IFileCleaner ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal sealed class FileCleaner : IFileCleaner
    {
        private readonly IFileFinder _fileFinder;

        [ImportingConstructor]
        public FileCleaner( IFileFinder fileFinder )
        {
            _fileFinder = fileFinder;
        }

        public List<string> Clean( IEnumerable<string> fileExtensions )
        {
            List<string> files = _fileFinder.Find( fileExtensions );

            var deleted = files.Where( DeleteFile ).ToList();

            return deleted;
        }

        private bool DeleteFile( string file )
        {
            if( !File.Exists( file ) )
            {
                return false;
            }

            File.SetAttributes( file, FileAttributes.Normal );
            File.Delete( file );

            return true;
        }
    }
}