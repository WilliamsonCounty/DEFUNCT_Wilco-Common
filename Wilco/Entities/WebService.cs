using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Wilco;

public static class WebService
{
	public static async Task<string> GetApiToken(string tokenUri, Dictionary<string, string> parameters, HttpClient client)
	{
		var response = await client.PostAsync(tokenUri, new FormUrlEncodedContent(parameters));
		var content = await response.Content.ReadAsStringAsync();
		var token = JsonConvert.DeserializeObject<Token>(content);

		return token?.AccessToken ?? throw new InvalidOperationException("An error was encountered during the retrieval of the bearer token.");
	}

	public static async Task<Response> GetResponse(string url, string token, HttpClient client)
	{
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

		using var response = await client.GetAsync(url);

		return new Response(response.Content.ReadAsStringAsync().Result, response.StatusCode, response.ReasonPhrase ?? "");
	}

	public record Response(string Result, HttpStatusCode HttpStatusCode, string? ReasonPhrase);

	/// <summary>
	/// Token class for deserialization
	/// </summary>
	public class Token
	{
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		[JsonProperty("expires_in")]
		public int ExpiresIn { get; set; }

		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }

		[JsonProperty("token_type")]
		public string TokenType { get; set; }
	}
}