﻿using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Wilco;

public static class WebService
{
	public static async Task<string> GetApiToken(string tokenUri, Dictionary<string, string> parameters)
	{
		using var client = new HttpClient();

		return await GetApiToken(tokenUri, parameters, client);
	}

	public static async Task<string> GetApiToken(string tokenUri, Dictionary<string, string> parameters, HttpClient client)
	{
		var response = await client.PostAsync(tokenUri, new FormUrlEncodedContent(parameters));
		var content = await response.Content.ReadAsStringAsync();
		var token = JsonConvert.DeserializeObject<Token>(content);

		//await cmd.ExecuteAsync();

		return token.AccessToken;
	}

	public static async Task<string> GetResponse(string url, string token)
	{
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

		using var client = new HttpClient();
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

		using var response = await client.GetAsync(url);

		return response.StatusCode switch
		{
			HttpStatusCode.OK           => await response.Content.ReadAsStringAsync(),
			HttpStatusCode.Unauthorized => "Unauthorized",
			_                           => "Error"
		};
	}

	public static async Task<string> GetResponse(string url, string token, HttpClient client)
	{
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

		using var response = await client.GetAsync(url);

		return response.StatusCode switch
		{
			HttpStatusCode.OK           => await response.Content.ReadAsStringAsync(),
			HttpStatusCode.Unauthorized => "Unauthorized",
			_                           => "Error" //Log.WriteLine(response.ReasonPhrase);
		};
	}

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