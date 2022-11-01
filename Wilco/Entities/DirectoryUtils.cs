namespace Wilco;

public abstract class DirectoryUtils
{
	public static readonly EnumerationOptions EnumerationOptions = new() { RecurseSubdirectories = false };
	public static readonly EnumerationOptions EnumerationOptionsRecursive = new() { RecurseSubdirectories = true };

	public DirectoryInfo DirectoryInfo { get; }
	public string FullName => DirectoryInfo.FullName;
	public string Name => DirectoryInfo.Name;
	public long TotalSize => DirectoryInfo.GetFiles("*", EnumerationOptionsRecursive).ToList().Sum(x => x.Length);

	protected DirectoryUtils(DirectoryInfo directoryInfo) => DirectoryInfo = directoryInfo;

	public static bool HasFiles(DirectoryInfo directoryInfo) => directoryInfo.GetFiles("*", EnumerationOptionsRecursive).Length > 0;

	/// <summary>
	/// Moves folder one level up.
	/// </summary>
	/// <param name="directoryInfo"></param>
	/// <returns>The number of folders that were deleted.</returns>
	public static DirectoryInfo MoveUpOneDirectory(DirectoryInfo directoryInfo)
	{
		var parent = directoryInfo.Parent;
		var newLocation = Path.Combine(parent.FullName, directoryInfo.FullName);

		if (Directory.Exists(newLocation))
			Directory.Delete(newLocation);
		
		directoryInfo.MoveTo(newLocation);

		return new DirectoryInfo(newLocation);
	}

	/// <summary>
	/// Deletes subfolders older than specified date.
	/// </summary>
	/// <param name="folder"></param>
	/// <param name="dateTime"></param>
	/// <returns>The number of folders that were deleted.</returns>
	public static int PurgeSubDirectories(DirectoryInfo folder, DateTime dateTime)
	{
		var counter = 0;

		folder.GetDirectories().Where(d => d.LastWriteTime < dateTime).ToList().ForEach(directory =>
		{
			directory.Delete(true);
			counter++;
		});

		return counter;
	}
}