using System.Collections.Generic;

namespace VisualStudioCleaner.Common.Domain.Cleaners
{
    public interface IDirectoryCleaner
    {
        List<string> Clean( IEnumerable<string> searchPatterns );
    }
}