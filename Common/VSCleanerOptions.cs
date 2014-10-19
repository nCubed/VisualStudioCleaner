using System;

namespace VisualStudioCleaner.Common
{
    [Flags]
    public enum VSCleanerOptions
    {
        /// <summary>
        /// Includes Files and Directories. Excludes Source Control Bindings.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Excludes Files even if <see cref="All"/> is included.
        /// </summary>
        ExcludeFiles = 1,

        /// <summary>
        /// Excludes Directories even if <see cref="All"/> is included.
        /// </summary>
        ExcludeDirectories = 2,

        /// <summary>
        /// Removes Source Control Bindings.
        /// </summary>
        RemoveSourceControlBindings = 4,

        /// <summary>
        /// Cleans everything; exclude options will still be honored.
        /// </summary>
        All = 8,
    }
}
