using Domain.Services;
using Microsoft.AspNetCore.DataProtection;

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
            return _protector.Unprotect(payload);
        }
    }
}
