using System.Collections.Generic;

namespace VisualStudioCleaner.Common.Domain.Cleaners
{
    public interface ISourceControlCleaner
    {
        string FileExtension { get; }
        List<string> Clean();
    }
}