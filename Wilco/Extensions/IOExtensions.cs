using System.Security.Cryptography;

namespace Wilco;

public static class IOExtensions
{
	public static string GetMD5Checksum(this FileInfo file)
	{
		using var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite,
			10 * 1000);

		var md5 = MD5.Create();
		var bytes = md5.ComputeHash(stream);

		return BitConverter.ToString(bytes).Replace("-", string.Empty).ToUpper();
	}

	public static string GetSha256Checksum(this FileInfo file)
	{
		using var stream = new BufferedStream(File.OpenRead(file.FullName), 10 * 1000);
		var sha = new SHA256Managed();
		var checksum = sha.ComputeHash(stream);

		return BitConverter.ToString(checksum).Replace("-", string.Empty).ToUpper();
	}

	/// <summary>
	///     Indicates if a directory contains any files.
	/// </summary>
	/// <param name="directoryInfo"></param>
	/// <param name="fileCount"></param>
	/// <returns>True or false and the total file count.</returns>
	public static bool HasFiles(this DirectoryInfo directoryInfo, out int fileCount)
	{
		fileCount = directoryInfo.GetFiles("*", SearchOption.AllDirectories).Length;

		return fileCount > 0;
	}
	
	/// <summary>
	/// Checks if a file is accessible.
	/// </summary>
	/// <param name="file"></param>
	/// <returns>True or false</returns>
	public static bool InUse(this FileInfo file) => !file.Exists || file.Attributes.HasFlag(FileAttributes.Hidden) || file.IsLocked();

	public static bool InUse(this DirectoryInfo directory)
	{
		var flag = false;

		_ = Parallel.ForEach(directory.GetFiles(), (file, loopState) =>
		{
			if (!file.InUse()) return;

			flag = true;
			loopState.Stop();
		});

		return flag;
	}

	///// <summary>
	/////     Creates a temp folder and moves current folder.
	///// </summary>
	///// <param name="evidenceFolder"></param>
	///// <returns></returns>
	//public static DirectoryInfo MoveToTempDirectory(this DirectoryInfo evidenceFolder) => MoveToTempDirectory(evidenceFolder, TempFolderOptions.CreateInSameFolder);

	///// <summary>
	/////     Creates a temp folder in the agency's evidence folder.
	///// </summary>
	///// <param name="evidenceFolder"></param>
	///// <param name="tempFolderOptions"></param>
	///// <returns>The directory that was moved to the temp folder.</returns>
	//public static DirectoryInfo MoveToTempDirectory(this DirectoryInfo evidenceFolder, TempFolderOptions tempFolderOptions)
	//{
	//	var tempDirectory = tempFolderOptions == TempFolderOptions.CreateUpOneLevel
	//		? Path.Combine(evidenceFolder.Parent.Parent.FullName, "!TEMP")
	//		: Path.Combine(evidenceFolder.FullName, "!TEMP");

	//	if (!Directory.Exists(tempDirectory)) _ = Directory.CreateDirectory(tempDirectory);

	//	evidenceFolder.Attributes &= ~FileAttributes.ReadOnly;
	//	evidenceFolder.MoveTo(Path.Combine(tempDirectory, evidenceFolder.Name));

	//	return evidenceFolder;
	//}

	/// <summary>
	///     Deletes subfolders based on number of days since last modified date.
	/// </summary>
	/// <param name="folder"></param>
	/// <param name="retentionInDays"></param>
	/// <returns>The number of folders that were deleted.</returns>
	public static int PurgeSubDirectories(this DirectoryInfo folder, int retentionInDays)
	{
		var purgeDate = retentionInDays == -1 ? DateTime.MaxValue : DateTime.Now.AddDays(-retentionInDays);
		var counter = 0;

		folder.GetDirectories().Where(d => d.LastWriteTime < purgeDate).ToList().ForEach(directory =>
		{
			directory.Delete(true);
			counter++;
		});

		return counter;
	}

	private static bool IsLocked(this FileInfo file)
	{
		FileStream? stream = null;

		try
		{
			stream = file.Open(FileMode.Open, FileAccess.ReadWrite);
		}
		catch (IOException)
		{
			return true;
		}
		finally
		{
			stream?.Close();
		}

		return false;
	}
}