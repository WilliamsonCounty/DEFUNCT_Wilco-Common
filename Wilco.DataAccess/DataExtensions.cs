using System.Data;
using System.Data.SqlClient;

namespace Wilco.DataAccess;

[Obsolete(
	"These methods are deprecated and will be removed from a future release. Please consider using an ORM, such as Entity Framework, or Dapper.")]
public static class DataExtensions
{
	/// <summary>
	///     Executes a command.
	/// </summary>
	/// <param name="command"></param>
	/// <param name="cs"></param>
	public static void Execute(this SqlCommand command)
	{
		using (var connection = new SqlConnection(ConnectionString.Value))
		{
			connection.Open();
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;
			_ = command.ExecuteNonQuery();
		}
	}

	/// <summary>
	///     Executes a command asyncronously.
	/// </summary>
	/// <param name="command"></param>
	/// <param name="cs"></param>
	public static async Task ExecuteAsync(this SqlCommand command)
	{
		using (var connection = new SqlConnection(ConnectionString.Value))
		{
			await connection.OpenAsync();
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = connection;
			_ = await command.ExecuteNonQueryAsync();
		}
	}

	/// <summary>
	///     Returns a data set.
	/// </summary>
	/// <param name="command"></param>
	/// <param name="cs"></param>
	/// <returns>DataSet</returns>
	public static DataSet GetDataSet(this SqlCommand command)
	{
		var ds = new DataSet();

		using (var connection = new SqlConnection(ConnectionString.Value))
		{
			connection.Open();
			command.Connection = connection;
			var da = new SqlDataAdapter(command);
			_ = da.Fill(ds);
		}

		return ds;
	}

	/// <summary>
	///     Returns a data table.
	/// </summary>
	/// <param name="command"></param>
	/// <param name="cs"></param>
	/// <returns>DataTable</returns>
	public static DataTable GetDataTable(this SqlCommand command)
	{
		var dt = new DataTable();

		using (var connection = new SqlConnection(ConnectionString.Value))
		{
			connection.Open();
			command.Connection = connection;
			var da = new SqlDataAdapter(command);
			_ = da.Fill(dt);
		}

		return dt;
	}

	/// <summary>
	///     Returns a single value.
	/// </summary>
	/// <param name="cmd"></param>
	/// <param name="cs"></param>
	/// <returns>Object</returns>
	public static object GetValue(this SqlCommand cmd)
	{
		using (var connection = new SqlConnection(ConnectionString.Value))
		{
			connection.Open();
			cmd.Connection = connection;

			return cmd.ExecuteScalar();
		}
	}

	/// <summary>
	///     Returns a single value.
	/// </summary>
	/// <param name="cmd"></param>
	/// <param name="cs"></param>
	/// <returns>Object</returns>
	public static async Task<object> GetValueAsync(this SqlCommand cmd)
	{
		using (var connection = new SqlConnection(ConnectionString.Value))
		{
			connection.Open();
			cmd.Connection = connection;

			return await cmd.ExecuteScalarAsync();
		}
	}
}