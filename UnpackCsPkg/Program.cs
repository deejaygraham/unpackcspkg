using System;
using System.IO;

namespace UnpackCsPkg
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("UnpackCsPkg <file>");
                return 1;
            }

            string package = args[0];

            if (!File.Exists(package))
            {
                Console.WriteLine("Package file : {0} does not exist", package);
                return 2;
            }

            try
            {
                Console.WriteLine("Unpacking {0}", package);

                string extractFolder = UnzipFileToFolder(package);

                foreach (string extractFile in Directory.GetFiles(extractFolder, "*.*x"))
                {
                    UnzipFileToFolder(extractFile);
                }

                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }

        private static string UnzipFileToFolder(string path)
        {
            string cleanPath = path.RemoveGuid();

            string containingFolder = Path.GetDirectoryName(path);
            string extractFolder = Path.Combine(containingFolder, Path.GetFileNameWithoutExtension(cleanPath));

            if (Directory.Exists(extractFolder))
                Directory.Delete(extractFolder, recursive: true);

            System.IO.Compression.ZipFile.ExtractToDirectory(path, extractFolder);

            RenameGuidFilesInFolder(extractFolder);

            return extractFolder;
        }

        private static void RenameGuidFilesInFolder(string folder)
        {
            foreach (string manifest in Directory.GetFiles(folder, "*.csman"))
            {
                string withoutGuid = manifest.RemoveGuid();

                if (!File.Exists(withoutGuid))
                {
                    File.Copy(manifest, withoutGuid);
                    File.Delete(manifest);
                }
            }

            foreach (string zipContent in Directory.GetFiles(folder, "*.cs*x"))
            {
                string withoutGuid = zipContent.RemoveGuid();

                if (!File.Exists(withoutGuid))
                {
                    File.Copy(zipContent, withoutGuid);
                    File.Delete(zipContent);
                }
            }

        }

    }
}
