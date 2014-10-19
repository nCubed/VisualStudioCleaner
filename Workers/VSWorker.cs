using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using VisualStudioCleaner.Common;
namespace VisualStudioCleaner.Workers
{
    [Export( typeof( IVSCleaner ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal sealed class VSWorker : IVSCleaner
    {
        private readonly IVSCleanerTools _tools;
        private readonly List<string> _directoryPatterns;
        private readonly List<string> _fileExtensions;

        public IReadOnlyCollection<string> DirectorySearchPatterns { get; private set; }

        public IReadOnlyCollection<string> FileExtensions { get; private set; }

        public VSCleanerOptions Options { get; private set; }


        [ImportingConstructor]
        public VSWorker( IVSCleanerTools tools )
        {
            _tools = tools;
            _directoryPatterns = new List<string>();
            _fileExtensions = new List<string>();
            DirectorySearchPatterns = new ReadOnlyCollection<string>( _directoryPatterns );
            FileExtensions = new ReadOnlyCollection<string>( _fileExtensions );
            Options = VSCleanerOptions.Default;
        }

        public void SetUp( string directorySearchPatterns, string fileExtensions, VSCleanerOptions options )
        {
            SetUpDirectories( directorySearchPatterns );

            SetUpFileExtensions( fileExtensions );

            Options = options;
        }

        public IVSCleanerResults DoWork()
        {
            var result = new VSCleanerResults();

            CleanDirectories( result );

            CleanFiles( result );

            CleanSourceControlBindings( result );

            return result;
        }

        private void CleanDirectories( VSCleanerResults result )
        {
            if( Options.HasOption( VSCleanerOptions.ExcludeDirectories ) )
            {
                result.Directories.Add( "No directories deleted; options excluded directories." );
                return;
            }

            if( Options.HasOption( VSCleanerOptions.All, VSCleanerOptions.Default ) )
            {
                var files = _tools.DirectoryCleaner.Clean( DirectorySearchPatterns );
                result.Directories.AddRange( files );
                return;
            }

            result.Directories.Add( "No directories deleted; options excluded directories." );
        }

        private void CleanFiles( VSCleanerResults result )
        {
            if( Options.HasOption( VSCleanerOptions.ExcludeFiles ) )
            {
                result.Files.Add( "No files deleted; options excluded files." );
                return;
            }

            if( Options.HasOption( VSCleanerOptions.All, VSCleanerOptions.Default ) )
            {
                var files = _tools.FileCleaner.Clean( FileExtensions );
                result.Files.AddRange( files );
                return;
            }

            result.Files.Add( "No files deleted; options excluded files." );
        }

        private void CleanSourceControlBindings( VSCleanerResults result )
        {
            if( Options.HasOption( VSCleanerOptions.All, VSCleanerOptions.RemoveSourceControlBindings ) )
            {
                var sol = _tools.SolutionFileCleaner.Clean();
                result.SouceControl.AddRange( sol );

                var proj = _tools.ProjectFileCleaner.Clean();
                result.SouceControl.AddRange( proj );

                return;
            }

            result.SouceControl.Add( "No source control cleaned; options excluded source control." );
        }

        private void SetUpDirectories( string directorySearchPatterns )
        {
            var directoryPatterns = StringHelper.Split( directorySearchPatterns, ";" );
            _directoryPatterns.Clear();
            _directoryPatterns.AddRange( directoryPatterns );
        }

        private void SetUpFileExtensions( string fileExtensions )
        {
            var extensions = StringHelper.Split( fileExtensions, ";" ).Select( StringHelper.CleanFileExtension );
            _fileExtensions.Clear();
            _fileExtensions.AddRange( extensions );
        }
    }
}