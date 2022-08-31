namespace Wilco;

public abstract class DirectoryBase
{
	public DirectoryInfo DirectoryInfo { get; }
	public string FullName => DirectoryInfo.FullName;
	public string Name => DirectoryInfo.Name;

	protected DirectoryBase(DirectoryInfo directoryInfo) => DirectoryInfo = directoryInfo;
}