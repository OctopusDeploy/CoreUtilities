using System.Security;

namespace Octopus.CoreUtilities.Extensions
{
    public static class StringExtensions
    {
        static SecureString ToSecureString(this string plainString)
        {
            if (plainString == null)
                return null;
 
            SecureString secureString = new SecureString();
            foreach (char c in plainString.ToCharArray())
            {
                secureString.AppendChar(c);
            }
            return secureString;
        }
    }
}