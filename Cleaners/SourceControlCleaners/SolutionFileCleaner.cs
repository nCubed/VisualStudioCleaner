using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using VisualStudioCleaner.Common.Domain.Cleaners;
using VisualStudioCleaner.Common.Domain.Finders;

namespace VisualStudioCleaner.Cleaners.SourceControlCleaners
{
    [Export( typeof( ISolutionFileCleaner ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal sealed class SolutionFileCleaner : SourceControlFileCleanerBase, ISolutionFileCleaner
    {
        private readonly IFileFinder _fileFinder;
        public override string FileExtension { get { return ".sln"; } }

        public const string SourceStart = "GlobalSection(TeamFoundationVersionControl)";
        public const string SourceEnd = "EndGlobalSection";

        [ImportingConstructor]
        public SolutionFileCleaner( IFileFinder fileFinder )
        {
            _fileFinder = fileFinder;
        }

        public List<string> Clean()
        {
            List<string> files = _fileFinder.Find( new[] { FileExtension } );

            List<string> cleaned = files.Where( Clean ).ToList();

            return cleaned;
        }

        private bool Clean( string filePath )
        {
            if( !File.Exists( filePath ) )
            {
                return false;
            }

            List<string> lines = File.ReadAllLines( filePath ).ToList();

            RemoveLines( lines );

            Save( filePath, lines );

            return true;
        }

        private void RemoveLines( List<string> lines )
        {
            List<int> indexes = FindIndexes( lines );

            // reverse the indexes to remove correct oridinal position; 
            // otherwise the list will shrink in size as each is removed 
            // and the wrong index will be removed.
            indexes.Reverse();

            foreach( int index in indexes )
            {
                lines.RemoveAt( index );
            }
        }

        private List<int> FindIndexes( List<string> lines )
        {
            var indexes = new List<int>();

            bool isInRange = false;

            for( int i = 0; i < lines.Count; i++ )
            {
                string line = lines[i].Trim();

                if( !isInRange && IsSourceControlStartLine( line ) )
                {
                    isInRange = true;
                    indexes.Add( i );
                    continue;
                }

                if( isInRange && IsSourceControlEndLine( line ) )
                {
                    isInRange = false;
                    indexes.Add( i );
                    continue;
                }

                if( isInRange )
                {
                    indexes.Add( i );
                }
            }

            return indexes;
        }

        private bool IsSourceControlStartLine( string line )
        {
            return line.StartsWith( SourceStart, StringComparison.OrdinalIgnoreCase );
        }

        private bool IsSourceControlEndLine( string line )
        {
            return line.Equals( SourceEnd, StringComparison.OrdinalIgnoreCase );
        }

        private void Save( string filePath, IEnumerable<string> lines )
        {
            base.SafeSave( filePath, () => File.WriteAllLines( filePath, lines ) );
        }
    }
}