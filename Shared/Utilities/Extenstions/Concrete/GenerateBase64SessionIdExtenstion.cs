
using Shared.Utilities.Extenstions.Interface;

namespace Shared.Utilities.Extenstions.Concrete;

public class GenerateBase64SessionIdExtenstion : IGenerateBase64SessionIdExtenstion
{
    public string GenerateBase64SessionId()
    {
        var base64 = "";
        var guid = Guid.NewGuid();
        var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz=";
        var id = guid.ToString().Replace("-", "");
        var parts = id.SplitInParts(4);
        foreach (var part in parts)
        {
            var base10 = Convert.ToInt32("0x" + part, 16);
            var val = base10 % 64;
            if (chars.Length > val) { base64 += chars[val]; }
            else { base64 += chars[chars.Length - 1]; }
        }
        return base64;
    }
}
