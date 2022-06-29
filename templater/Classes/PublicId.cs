using HashidsNet;

namespace templater.Classes;

static class PublicId
{
    const string salt = "A6CC0958-DCAC-41C7-A38A-E56506CE9DA8";
    private static readonly Hashids _hasher = new(salt, 4, "abcdefghijklmnopqrstuvwxyz0123456789");

    internal static string Encode(int value)
    {
        return _hasher.Encode(value);
    }

    internal static int Decode(string hash)
    {
        try
        {
            return _hasher.Decode(hash.ToLower())[0];
        }
        catch
        {
            return int.MinValue;
        }
    }
}
