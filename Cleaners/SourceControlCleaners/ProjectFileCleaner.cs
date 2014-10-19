using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;
using VisualStudioCleaner.Common.Domain.Cleaners;
using VisualStudioCleaner.Common.Domain.Finders;

namespace VisualStudioCleaner.Cleaners.SourceControlCleaners
{
    [Export( typeof( IProjectFileCleaner ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal sealed class ProjectFileCleaner : SourceControlFileCleanerBase, IProjectFileCleaner
    {
        private readonly IFileFinder _fileFinder;
        public override string FileExtension { get { return ".csproj"; } }


        [ImportingConstructor]
        public ProjectFileCleaner( IFileFinder fileFinder )
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
            XDocument doc;
            if( !TryCreateDoc( filePath, out doc ) )
            {
                return false;
            }

            RemoveNodes( doc.Root );

            Save( filePath, doc );

            return true;
        }

        private bool TryCreateDoc( string filePath, out XDocument doc )
        {
            try
            {
                doc = XDocument.Load( filePath );
                if( doc.Root != null )
                {
                    return true;
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch { }

            doc = null;
            return false;
        }

        private void RemoveNodes( XElement doc )
        {
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";

            var nodes = new[] { "SccProjectName", "SccLocalPath", "SccAuxPath", "SccProvider" };

            foreach( string node in nodes )
            {
                doc.Descendants( ns + node ).Remove();
            }
        }

        private void Save( string filePath, XDocument doc )
        {
            base.SafeSave( filePath, () => doc.Save( filePath ) );
        }
    }
}
