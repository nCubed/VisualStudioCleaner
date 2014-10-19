using System.Linq;

namespace VisualStudioCleaner.Common
{
    public static class VSCleanerOptionsExtensions
    {
        public static bool HasOption( this VSCleanerOptions options, params VSCleanerOptions[] cleanerOptions )
        {
            return cleanerOptions.Any( opt => options == opt || options.HasFlag( opt ) );
        }
    }
}