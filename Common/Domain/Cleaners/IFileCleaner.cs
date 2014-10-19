using System.Collections.Generic;

namespace VisualStudioCleaner.Common.Domain.Cleaners
{
    public interface IFileCleaner
    {
        List<string> Clean( IEnumerable<string> fileExtensions );
    }
}