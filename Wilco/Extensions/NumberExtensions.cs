namespace Wilco.NET6;

public static class NumberExtensions
{
	/// <summary>
	/// Divides a number by a provided divisor.
	/// </summary>
	/// <param name="i"></param>
	/// <param name="dividend"></param>
	/// <param name="divisor"></param>
	/// <returns>The result of the division. Will return zero if the divisor is zero.</returns>
	public static decimal DivideBy(this int dividend, int divisor) =>
		divisor.IsGreaterThanZero() ? dividend / divisor : throw new DivideByZeroException("Divisor must be greater than 0.");

	/// <summary>
	/// Evaluates a number to determine if it is greater than or equal to another number.
	/// </summary>
	/// <param name="i"></param>
	/// <param name="value"></param>
	/// <returns>Boolean</returns>
	public static bool IsGreaterThan(this int i, int value) => i > value;

	/// <summary>
	/// Evaluates a number to determine if it is greater than or equal to another number.
	/// </summary>
	/// <param name="i"></param>
	/// <param name="value"></param>
	/// <returns>Boolean</returns>
	public static bool IsGreaterThanOrEqualTo(this int i, int value) => i >= value;

	/// <summary>
	/// Evaluates a number to determine if it is greater than or equal to zero.
	/// </summary>
	/// <param name="i"></param>
	/// <returns>Boolean</returns>
	public static bool IsGreaterThanOrEqualToZero(this int i) => i >= 0;

	/// <summary>
	/// Evaluates a number to determine if it is greater than zero.
	/// </summary>
	/// <param name="i"></param>
	/// <returns>Boolean</returns>
	public static bool IsGreaterThanZero(this int i) => i > 0;

	/// <summary>
	/// Evaluates a number to determine if it is less than another number.
	/// </summary>
	/// <param name="i"></param>
	/// <returns>Boolean</returns>
	public static bool IsLessThan(this int i, int value) => i < value;

	/// <summary>
	/// Evaluates a number to determine if it is less than or equal to another number.
	/// </summary>
	/// <param name="i"></param>
	/// <returns>Boolean</returns>
	public static bool IsLessThanOrEqualTo(this int i, int value) => i <= value;

	/// <summary>
	/// Evaluates a number to determine if it is less than or equal to zero.
	/// </summary>
	/// <param name="i"></param>
	/// <returns>Boolean</returns>
	public static bool IsLessThanOrEqualToZero(this int i) => i <= 0;

	/// <summary>
	/// Takes a decimal and returns an integral number greater than or equal to value.
	/// </summary>
	/// <param name="value"></param>
	/// <returns>An integral number greater than or equal to value. The returned value is an integer.</returns>
	public static int ToCeiling(this decimal value) => (int)Math.Ceiling(value);
}