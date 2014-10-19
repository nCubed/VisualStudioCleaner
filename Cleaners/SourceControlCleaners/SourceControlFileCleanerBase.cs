using System;
using System.IO;

namespace VisualStudioCleaner.Cleaners.SourceControlCleaners
{
    internal abstract class SourceControlFileCleanerBase
    {
        public abstract string FileExtension { get; }

        // ReSharper disable once EmptyConstructor
        protected SourceControlFileCleanerBase()
        { }

        /// <summary>
        /// Safely saves a file by ensuring the original file is not ReadOnly which would cause
        /// an exception if attempting to write over the existing file.
        /// </summary>
        /// <param name="filePath">The fully qualified path to the file.</param>
        /// <param name="saveMethod">The method to save the file contents.</param>
        protected internal void SafeSave( string filePath, Action saveMethod )
        {
            FileAttributes originalAttributes = File.GetAttributes( filePath );

            File.SetAttributes( filePath, FileAttributes.Normal );

            saveMethod();

            File.SetAttributes( filePath, originalAttributes );
        }
    }
}