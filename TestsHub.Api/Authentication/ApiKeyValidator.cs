using System;
using System.Security.Cryptography;
using System.Text;

namespace TestHub.Api.Authentication
{
    public class ApiKeyValidator
    {
        public static string GenerateApiKey(string org)
        {
            var key = new string('$', 100) + org.ToLower() +  "-" + "hackers go away!" + "-" + new string('~', 100);
            var md5 = SHA1.Create();
            md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            return ToHex(md5.Hash, false);
        }

        static string ToHex(byte[] bytes, bool upperCase)
        {
            var result = new StringBuilder(bytes.Length * 2);

            for (var i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
            }

            return result.ToString();
        }

        public static bool IsKeyValid(string apiKey, string org)
        {
            var key = GenerateApiKey(org);
            if (apiKey.Equals(key))
            {
                return true;
            }
            return false;
        }
    }
}
