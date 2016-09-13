using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmpCsPkg
{
	class Program
	{
		static int Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("CmpCsPkg <source-folder> <target-folder>");
				return 1;
			}

			string sourceFolder = args[0];

			if (!Directory.Exists(sourceFolder))
			{
				Console.WriteLine("Source folder : {0} does not exist", sourceFolder);
				return 2;
			}

			string targetFolder = args[1];

			if (!Directory.Exists(targetFolder))
			{
				Console.WriteLine("Target folder : {0} does not exist", targetFolder);
				return 3;
			}

			try
			{
				Console.WriteLine("Comparing {0} to {1}", sourceFolder, targetFolder);
				Console.WriteLine();

				const string AllFiles = "*.*";
				const SearchOption depth = SearchOption.AllDirectories;

				var sourceFiles = LoadFilesFrom(sourceFolder, AllFiles, depth);
				var targetFiles = LoadFilesFrom(targetFolder, AllFiles, depth);

				var comparison = new FileComparer();

				ReportOnMissingFiles(sourceFiles, targetFiles, comparison);
				ReportOnMissingFiles(targetFiles, sourceFiles, comparison);

				ReportOnCommonFiles(sourceFiles, targetFiles, comparison);

				Console.WriteLine("Done");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return 0;
		}

		private static IEnumerable<File> LoadFilesFrom(string path, string mask, SearchOption depth)
		{
			var files = new List<File>();

			foreach (var file in Directory.GetFiles(path, mask, depth))
			{
				files.Add(new File(file, path));
			}

			files.Sort(new FileNameComparer());

			return files;
		}

		private static void ReportOnMissingFiles(IEnumerable<File> sourceFiles, IEnumerable<File> targetFiles, IEqualityComparer<File> comparison)
		{
			var inSourceOnly = sourceFiles.Except(targetFiles, comparison);

			if (inSourceOnly.Any())
			{
				Console.WriteLine("Files in \'{0}\' ONLY", sourceFiles.First().BaseFolder);
				Console.WriteLine();

				string currentDirectory = string.Empty;

				foreach(var source in inSourceOnly)
				{
					if (currentDirectory != source.SubFolder)
					{
						currentDirectory = source.SubFolder;
						Console.WriteLine("[" + currentDirectory + "]");
					}

					Console.WriteLine("\t{0}", source.Name);
				}

				Console.WriteLine();
			}
		}

		private static void ReportOnCommonFiles(IEnumerable<File> sourceFiles, IEnumerable<File> targetFiles, IEqualityComparer<File> comparison)
		{
			var inCommon = sourceFiles.Intersect(targetFiles, comparison);

			// names agree - do deeper analysis...
			if (inCommon.Any())
			{
				Console.WriteLine("Files in common:");
				Console.WriteLine();

				int differences = 0;
				int differencesInThisDirectory = 0;
				string currentDirectory = string.Empty;

				foreach (File common in inCommon)
				{
					if (currentDirectory != common.SubFolder)
					{
						currentDirectory = common.SubFolder;
						differencesInThisDirectory = 0;
					}

					File sourceFile = common;
					File targetFile = targetFiles.FirstOrDefault(x => x.SubFolderAndName == sourceFile.SubFolderAndName);

					if (sourceFile.Length != targetFile.Length)
					{
						++differences;
						++differencesInThisDirectory;

						if (differencesInThisDirectory == 1)
							Console.WriteLine("[" + currentDirectory + "]");

						Console.WriteLine("\t{0} - {1} : {2}", sourceFile.SubFolderAndName, sourceFile.Length, targetFile.Length);
					}
				}

				Console.WriteLine("{0} differences", differences);
			}
		}
	}


}
