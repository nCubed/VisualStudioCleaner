using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualStudioCleaner.Common
{
    public static class StringHelper
    {
        /// <summary>
        /// Takes a string containing substrings that are delimited by a
        /// specified seperator and converts it into a collection of strings.
        /// </summary>
        /// <param name="str">The string to split.</param>
        /// <param name="separator">The deliminator to split the string on.</param>
        public static List<string> Split( string str, string separator )
        {
            var query = str.Split( new[] { separator }, StringSplitOptions.RemoveEmptyEntries )
                .Where( x => !string.IsNullOrWhiteSpace( x ) )
                .Select( x => x.Trim() )
                .OrderBy( x => x )
                .ToList();

            return query;
        }

        public static string CleanFileExtension( string fileExtension )
        {
            if( string.IsNullOrWhiteSpace( fileExtension ) )
            {
                throw new ArgumentNullException( "fileExtension" );
            }

            string ext = fileExtension.Trim();

            if( !ext.StartsWith( ".", StringComparison.OrdinalIgnoreCase ) )
            {
                ext = "." + ext;
            }

            return ext;
        }
    }
}