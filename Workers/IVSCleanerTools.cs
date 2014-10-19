using VisualStudioCleaner.Common.Domain.Cleaners;

namespace VisualStudioCleaner.Workers
{
    public interface IVSCleanerTools
    {
        IFileCleaner FileCleaner { get; }
        IDirectoryCleaner DirectoryCleaner { get; }
        IProjectFileCleaner ProjectFileCleaner { get; }
        ISolutionFileCleaner SolutionFileCleaner { get; }
    }
}