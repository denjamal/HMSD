using System;
using System.Security.Cryptography;
using System.Text;
using Domain.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;

namespace Infrastructure.Services
{
    public class Cipher : ICipher
    {
        private readonly IDataProtector _protector;

        public Cipher(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("HMSD.test.v1");
        }

        public string Encrypt(string payload)
        {
            return _protector.Protect(payload);
        }

        public string Decrypt(string payload)
        {
            try
            {
                return _protector.Unprotect(payload);
            }
            catch (CryptographicException e)
            {
                var persistedProtector = _protector as IPersistedDataProtector;
                if (persistedProtector == null)
                {
                    throw new Exception("Can't call DangerousUnprotect.");
                }

                var plain = persistedProtector.DangerousUnprotect(
                    WebEncoders.Base64UrlDecode(payload),
                    true,
                    out var migrate,
                    out var revoked);
                return Encoding.UTF8.GetString(plain);
            }
            
        }
    }
}
