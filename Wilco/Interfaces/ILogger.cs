namespace Wilco;

public interface ILogger
{
	bool AddTimestamp { get; }
	bool VerboseErrorOutput { get; }
	bool WriteToConsole { get; }

	string PrintErrors();

	void LogInformation(string message);

	void LogError(Exception ex);
}