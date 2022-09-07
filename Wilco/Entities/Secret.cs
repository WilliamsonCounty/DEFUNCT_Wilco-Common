using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Wilco.Azure.KeyVault;

public static class Secret
{
	public static async Task<string> GetAsync(string keyVaultName, string keyName)
	{
		Uri kvUri = new($"https://{keyVaultName}.vault.azure.net/");
		var client = new SecretClient(kvUri, new DefaultAzureCredential());
		var secret = await client.GetSecretAsync(keyName);

		return secret.Value.Value;
	}
}