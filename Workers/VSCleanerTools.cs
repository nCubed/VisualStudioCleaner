using System.ComponentModel.Composition;
using VisualStudioCleaner.Common.Domain.Cleaners;

namespace VisualStudioCleaner.Workers
{
    [Export( typeof( IVSCleanerTools ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal sealed class VSCleanerTools : IVSCleanerTools
    {
        public IFileCleaner FileCleaner { get; private set; }

        public IDirectoryCleaner DirectoryCleaner { get; private set; }

        public IProjectFileCleaner ProjectFileCleaner { get; private set; }

        public ISolutionFileCleaner SolutionFileCleaner { get; private set; }

        [ImportingConstructor]
        public VSCleanerTools(
            IFileCleaner fileCleaner,
            IDirectoryCleaner directoryCleaner,
            IProjectFileCleaner projectFileCleaner,
            ISolutionFileCleaner solutionFileCleaner )
        {
            SolutionFileCleaner = solutionFileCleaner;
            ProjectFileCleaner = projectFileCleaner;
            DirectoryCleaner = directoryCleaner;
            FileCleaner = fileCleaner;
        }
    }
}
