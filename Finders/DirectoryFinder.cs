using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using VisualStudioCleaner.Common.Domain;
using VisualStudioCleaner.Common.Domain.Finders;

namespace VisualStudioCleaner.Finders
{
    [Export( typeof( IDirectoryFinder ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal sealed class DirectoryFinder : IDirectoryFinder
    {
        public string RootDirectory { get; private set; }

        public DirectoryFinder( string rootDirectory )
        {
            RootDirectory = rootDirectory;
        }

        [ImportingConstructor]
        public DirectoryFinder( IRootDirectory rootDirectory )
            : this( rootDirectory.RootDirectory )
        { }

        public List<string> Find( IEnumerable<string> searchPatterns )
        {
            var results = searchPatterns
                .SelectMany( x => Directory.GetDirectories( RootDirectory, x, SearchOption.AllDirectories ) )
                .Distinct()
                .ToList();

            return results;
        }
    }
}
