//using System;
//using System.Threading.Tasks;
//using Azure.Identity;
//using Azure.Security.KeyVault.Secrets;

//namespace Wilco.Azure.KeyVault
//{
//	public static class Secret
//	{
//		public static async Task<string> GetAsync(string keyVaultName, string keyName)
//		{
//			var kvUri = $"https://{keyVaultName}.vault.azure.net";
//			var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
//			var secret = await client.GetSecretAsync(keyName);

//			return secret.Value.Value;
//		}
//	}
//}