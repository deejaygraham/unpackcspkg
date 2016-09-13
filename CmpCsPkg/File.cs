using System.IO;

namespace CmpCsPkg
{
	[System.Diagnostics.DebuggerDisplay("{SubFolderAndName}")]
	public class File
	{
		public File(string fullPath, string baseFolder)
		{
			this.FullName = fullPath;
			this.BaseFolder = baseFolder;
			this.Name = Path.GetFileName(fullPath);
			this.SubFolder = Path.GetDirectoryName(fullPath).Replace(this.BaseFolder, "").Substring(1);
			this.SubFolderAndName = this.FullName.Replace(this.BaseFolder, "").Substring(1);

			this.Length = new FileInfo(fullPath).Length;
		}

		public string Name { get; private set; }

		public string FullName { get; private set; }

		public string SubFolder { get; private set; }

		public string SubFolderAndName { get; private set; }

		public string BaseFolder { get; private set; }

		public long Length { get; private set; }
	}

}
