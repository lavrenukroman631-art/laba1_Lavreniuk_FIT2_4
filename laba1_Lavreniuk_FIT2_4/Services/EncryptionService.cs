
using System.Security.Cryptography;
using System.Text;
using Microsoft.Maui.Storage;

namespace laba1_Lavreniuk_FIT2_4.Services;

public class EncryptionService
{
    private const string KeyName = "StudentEncryptionKey";

    private async Task<byte[]> GetKeyAsync()
    {
        string? savedKey = await SecureStorage.Default.GetAsync(KeyName);

        if (string.IsNullOrEmpty(savedKey))
        {
            byte[] newKey = RandomNumberGenerator.GetBytes(32);
            savedKey = Convert.ToBase64String(newKey);

            await SecureStorage.Default.SetAsync(KeyName, savedKey);
        }

        return Convert.FromBase64String(savedKey);
    }

    public async Task<string> EncryptAsync(string text)
    {
        byte[] key = await GetKeyAsync();

        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();

        using ICryptoTransform encryptor = aes.CreateEncryptor();

        byte[] plainBytes = Encoding.UTF8.GetBytes(text);
        byte[] encryptedBytes = encryptor.TransformFinalBlock(
            plainBytes, 0, plainBytes.Length);

        byte[] result = new byte[aes.IV.Length + encryptedBytes.Length];

        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(
            encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);

        return Convert.ToBase64String(result);
    }

    public async Task<string> DecryptAsync(string encryptedText)
    {
        if (string.IsNullOrEmpty(encryptedText))
            return string.Empty;

        byte[] key = await GetKeyAsync();
        byte[] data = Convert.FromBase64String(encryptedText);

        using Aes aes = Aes.Create();
        aes.Key = key;

        byte[] iv = new byte[16];
        byte[] encryptedBytes = new byte[data.Length - iv.Length];

        Buffer.BlockCopy(data, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(data, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

        aes.IV = iv;

        using ICryptoTransform decryptor = aes.CreateDecryptor();

        byte[] decryptedBytes = decryptor.TransformFinalBlock(
            encryptedBytes, 0, encryptedBytes.Length);

        return Encoding.UTF8.GetString(decryptedBytes);
    }
}
