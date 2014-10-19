using System.Collections.Generic;

namespace VisualStudioCleaner.Common.Domain.Finders
{
    public interface IFileFinder : IFinder
    {
        /// <summary>
        /// Finds all files matching the provided <paramref name="fileExtensions"/>. This method
        /// will only find files matching the full file extension provided. i.e.,
        /// if the <paramref name="fileExtensions"/> contains ".xls", then only files with 
        /// the ".xls" extension will be found; file extensions such as ".xlsx" will be ignored.
        /// </summary>
        /// <param name="fileExtensions">The file extensions to find.</param>
        /// <returns>A collection of files found from the file extensions provided.</returns>
        List<string> Find( IEnumerable<string> fileExtensions );
    }
}