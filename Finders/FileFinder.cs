using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using VisualStudioCleaner.Common.Domain;
using VisualStudioCleaner.Common.Domain.Finders;

namespace VisualStudioCleaner.Finders
{
    [Export( typeof( IFileFinder ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal sealed class FileFinder : IFileFinder
    {
        private readonly Lazy<string[]> _allFiles;

        public string RootDirectory { get; private set; }

        public FileFinder( string rootDirectory )
        {
            RootDirectory = rootDirectory;

            _allFiles = new Lazy<string[]>( GetAllFiles );
        }

        [ImportingConstructor]
        public FileFinder( IRootDirectory rootDirectory )
            : this( rootDirectory.RootDirectory )
        { }

        public List<string> Find( IEnumerable<string> fileExtensions )
        {
            string[] files = _allFiles.Value;
            var matchedFiles = files
                .Where( x => fileExtensions.Any( ext => x.EndsWith( ext, StringComparison.OrdinalIgnoreCase ) ) )
                .ToList();

            return matchedFiles;
        }

        private string[] GetAllFiles()
        {
            string[] files = Directory.GetFiles( RootDirectory, "*", SearchOption.AllDirectories );

            return files;
        }
    }
}