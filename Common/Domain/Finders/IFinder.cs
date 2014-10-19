namespace VisualStudioCleaner.Common.Domain.Finders
{
    public interface IFinder
    {
        /// <summary>
        /// Gets the root directory that is used for all finder operations.
        /// </summary>
        string RootDirectory { get; }
    }
}