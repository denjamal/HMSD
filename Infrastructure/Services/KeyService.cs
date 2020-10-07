using System;
using Domain.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace Infrastructure.Services
{
    public class KeyService : IKeyService
    {
        private readonly IKeyManager _keyManager;

        public KeyService(IKeyManager keyManager)
        {
            _keyManager = keyManager;
        }

        public void RotateEncryptionKey()
        {
            var dateTimeNow = DateTime.UtcNow;
            _keyManager.RevokeAllKeys(dateTimeNow, "manual rotating");
            _keyManager.CreateNewKey(dateTimeNow, dateTimeNow
                .AddDays(30));
        }
    }
}
