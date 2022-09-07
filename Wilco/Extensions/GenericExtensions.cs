namespace Wilco;

public static class GenericExtensions
{
	/// <summary>
	///     Indicates if a given value is null or empty.
	/// </summary>
	/// <param name="input"></param>
	/// <param name="defaultValue"></param>
	/// <returns>The original input, or a given default value if the input is null.</returns>
	public static T ValueOrDefault<T>(this T input, T defaultValue) => input is null ? defaultValue : input;
}