using System;
using System.Collections.Generic;

namespace CmpCsPkg
{
	public class FileComparer : IEqualityComparer<File>, IComparer<File>
	{
		public bool Equals(File x, File y)
		{
			return x.SubFolderAndName.Equals(y.SubFolderAndName, StringComparison.InvariantCultureIgnoreCase);
		}

		public int GetHashCode(File obj)
		{
			return 0;
		}

		public int Compare(File x, File y)
		{
			int compare = x.SubFolder.CompareTo(y.SubFolder);

			if (compare == 0)
				return x.Name.CompareTo(y.Name);

			return compare;
		}
	}
}
