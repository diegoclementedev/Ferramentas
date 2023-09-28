namespace EncryptedColumnsEFCore;

public static class EncrypterHelper
{
    private const string KeySecret = "fedaf7d8863b48e197b9287d492b708e";
    public static string EncryptData(string data)
    {
        return AesOperation.EncryptString(KeySecret, data);
    }

    public static string DecryptData(string encryptedData)
    {
        return AesOperation.DecryptString(KeySecret, encryptedData);
    }
}