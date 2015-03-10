using System;
using System.IO;

namespace UnpackCsPkg
{
    public static class StringExtensions
    {
        public static string RemoveGuid(this string path)
        {
            string containingFolder = Path.GetDirectoryName(path);
            string filenameOnly = Path.GetFileNameWithoutExtension(path);

            // assuming no other underscores...
            const char Underscore = '_';

            int firstUnderscore = filenameOnly.IndexOf(Underscore);

            // is the name in the format <name>_guid ?
            if (firstUnderscore > 0)
            {
                string candidateGuid = filenameOnly.Substring(firstUnderscore + 1);

                if (candidateGuid.IsInGuidFormat())
                {
                    filenameOnly = filenameOnly.Substring(0, firstUnderscore);
                }
            }
            else
            {
                // maybe it's all guid ?
                string filename = Path.GetFileNameWithoutExtension(path);

                if (filename.IsInGuidFormat())
                {
                    // take the name from the containing folder.
                    filenameOnly = new DirectoryInfo(containingFolder).Name;
                }
            }

            return Path.Combine(containingFolder, filenameOnly + Path.GetExtension(path));
        }

        public static bool IsInGuidFormat(this string candidate)
        {
            try
            {
                new Guid(candidate);
                return true;
            }
            catch
            {
                // not really a guid..
            }

            return false;
        }
    }
}
