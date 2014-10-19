using System.Collections.Generic;

namespace VisualStudioCleaner.UnitTestTools
{
    public static class FileExt
    {
        public static IReadOnlyCollection<string> AllExtensions = new List<string>
        {
            Dir, File, 
            Ext1, Ext2, Ext3, Ext4, Ext5,
            Ext6, Ext7, Ext8, Ext9, Ext10,
        }.AsReadOnly();

        /// <summary>
        /// returns: .dir
        /// </summary>
        public const string Dir = ".dir";

        /// <summary>
        /// returns: .file
        /// </summary>
        public const string File = ".file";

        /// <summary>
        /// returns: .ext1
        /// </summary>
        public const string Ext1 = ".ext1";

        /// <summary>
        /// returns: .ext2
        /// </summary>
        public const string Ext2 = ".ext2";

        /// <summary>
        /// returns: .ext3
        /// </summary>
        public const string Ext3 = ".ext3";

        /// <summary>
        /// returns: .ext4
        /// </summary>
        public const string Ext4 = ".ext4";

        /// <summary>
        /// returns: .ext5
        /// </summary>
        public const string Ext5 = ".ext5";

        /// <summary>
        /// returns: .ext6
        /// </summary>
        public const string Ext6 = ".ext6";

        /// <summary>
        /// returns: .ext7
        /// </summary>
        public const string Ext7 = ".ext7";

        /// <summary>
        /// returns: .ext8
        /// </summary>
        public const string Ext8 = ".ext8";

        /// <summary>
        /// returns: .ext9
        /// </summary>
        public const string Ext9 = ".ext9";

        /// <summary>
        /// returns: .ext10
        /// </summary>
        public const string Ext10 = ".ext10";
    }
}