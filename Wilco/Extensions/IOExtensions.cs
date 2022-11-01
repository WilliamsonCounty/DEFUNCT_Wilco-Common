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
		var sha = SHA256.Create();
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
		fileCount = directoryInfo.GetFiles("*", DirectoryUtils.EnumerationOptionsRecursive).Length;

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