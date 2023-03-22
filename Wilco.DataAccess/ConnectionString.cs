namespace Wilco.DataAccess;

public struct ConnectionString
{
	private static string? _value;

	public static string Value
	{
		get => _value ?? throw new NullReferenceException("Connection string has not been set.");
		private set => _value = value;
	}

	public static void Set(string connectionString) => Value = connectionString;

	public static void Set(string server, string database) =>
		_value = $"Data Source={server};" + "Connection Timeout=300;" + $"Initial Catalog={database};" + "Integrated Security=SSPI;";

	public static void Set(string server, string database, string username, string password) =>
		_value = $"Data Source={server};" + "Connection Timeout=300;" + $"Initial Catalog={database};" + $"User ID={username};" + $"Password='{password}';";

	public override string ToString() => Value;
}