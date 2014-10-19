using System.Collections.Generic;

namespace VisualStudioCleaner.Common.Domain.Finders
{
    public interface IDirectoryFinder : IFinder
    {
        /// <summary>
        /// Finds directories using the search pattern from Directory.GetFiles.
        /// See the Remarks section on MSDN: http://msdn.microsoft.com/en-us/library/ms143316(v=vs.110).aspx
        /// for how to create a search pattern.
        /// </summary>
        /// <param name="searchPatterns">The search patterns to use for finding directories.</param>
        /// <returns>A collection of directories found based off the search pattern provided.</returns>
        List<string> Find( IEnumerable<string> searchPatterns );
    }
}