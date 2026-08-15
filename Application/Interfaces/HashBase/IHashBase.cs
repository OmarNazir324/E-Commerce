namespace Application.Interfaces.HashBase;

public interface IHashBase
{
    string Encrypt(string encryptString);
    string Decrypt(string cipherText);
}
