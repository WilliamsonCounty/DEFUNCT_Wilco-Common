using System.Reflection;
using System.Text;

namespace Wilco;

public enum LogLevel
{
	Info,
	Warning,
	Error
}

public class JsonLogger : ILogger
{
	private static readonly string AppLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
	private static SemaphoreSlim? SemaphoreSlim;
	private static readonly string LogFolderPath = Path.Combine(AppLocation, LogFolderName);
	private static StringBuilder? ErrorCache;
	private const string LogFolderName = "LOGS";

	private static string LogName = $"{DateTime.Now:MMddyyyy}.txt";
	private static string? LogPath;

	public bool AddTimestamp { get; }
	public bool VerboseErrorOutput { get; }
	public bool WriteToConsole { get; }
	public bool WriteToHtml { get; }

	public JsonLogger(bool verboseErrorOutput = false, bool writeToConsole = false, bool writeToHtml = false, bool addTimestamp = true, string logFileName = "")
	{
		(VerboseErrorOutput, WriteToConsole, WriteToHtml, AddTimestamp) = (verboseErrorOutput, writeToConsole, writeToHtml, addTimestamp);

		if (!string.IsNullOrWhiteSpace(logFileName))
			LogName = logFileName;
			
		LogPath = Path.Combine(LogFolderPath, LogName);
		ErrorCache = new StringBuilder();
		SemaphoreSlim = new SemaphoreSlim(1, 1);

		if (Directory.Exists(LogFolderPath))
			return;

		SemaphoreSlim.Wait();
		_ = Directory.CreateDirectory(LogFolderPath);
		_ = SemaphoreSlim.Release();

		Cleanup();
	}

	private static void Cleanup() => Cleanup(30);

	private static void Cleanup(int lastModifiedInDays)
	{
		foreach (var file in new DirectoryInfo(LogFolderPath).GetFiles())
			if (file.LastWriteTime < DateTime.Now.AddDays(-lastModifiedInDays))
				File.Delete(file.FullName);
	}

	private void Write(string output, LogEntryType logEntryType)
	{
		output = AddTimestamp ? $"[{DateTime.Now}] {output}" : output;

		WriteToText(output);

		if (WriteToHtml)
			WriteHtml(output, logEntryType);

		if (WriteToConsole)
			WriteConsole(output, logEntryType);
	}

	private static void WriteConsole(string output, LogEntryType logEntryType)
	{
		Console.ForegroundColor = ConsoleColor.Green;

		if (logEntryType == LogEntryType.Error)
			Console.ForegroundColor = ConsoleColor.Red;

		Console.WriteLine(output);
	}

	private static void WriteHtml(string output, LogEntryType logEntryType)
	{
		SemaphoreSlim.Wait();

		var divType = logEntryType == LogEntryType.Error ? "<div style=\"color:#990000;\">" : "<div>";
		var completeLine = $"{divType}{output}</div>";

		using (var fs = new FileStream($"{LogPath.Replace(".txt", "")}.html", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
		using (var sw = new StreamWriter(fs))
		{
			sw.WriteLine(completeLine);
		}

		_ = SemaphoreSlim.Release();
	}

	private static void WriteToText(string output)
	{
		SemaphoreSlim.Wait();

		using (var fs = new FileStream(LogPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
		using (var sw = new StreamWriter(fs))
		{
			sw.WriteLine(output);
		}

		_ = SemaphoreSlim.Release();
	}

	/// <summary>
	///     Writes a new error to the log.
	/// </summary>
	/// <param name="ex"></param>
	public void LogError(Exception ex)
	{
		var output = VerboseErrorOutput ? ex.StackTrace : ex.Message;

		Write($"ERROR {output}", LogEntryType.Error);

		_ = ErrorCache.AppendLine(output);
	}

	/// <summary>
	///     Writes a new message to the log.
	/// </summary>
	/// <param name="output"></param>
	public void LogInformation(string output) => Write($"INFORMATION {output}", LogEntryType.Information);

	public string PrintErrors() => ErrorCache.ToString();
}