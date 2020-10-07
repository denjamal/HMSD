namespace Domain.Services
{
    public interface ICipher
    {
        string Encrypt(string payload);
        string Decrypt(string payload);
    }
}
