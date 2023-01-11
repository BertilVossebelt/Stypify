using System;
using System.Security.Cryptography;

namespace TypingApp.Services.PasswordHash;

public sealed class PasswordHash
{
    // Properties for wanted password hashing settings, like hash iterations. 
    const int SaltSize = 16, HashSize = 20, HashIter = 10000;
    readonly byte[] _salt, _hash;
    
    // Turn the user-entered password into a hashed password.
    public PasswordHash(string password)
    {
        new RNGCryptoServiceProvider().GetBytes(_salt = new byte[SaltSize]);
        _hash = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
    }
    
    public PasswordHash(byte[] hashBytes)
    {
        Array.Copy(hashBytes, 0, _salt = new byte[SaltSize], 0, SaltSize);
        Array.Copy(hashBytes, SaltSize, _hash = new byte[HashSize], 0, HashSize);
    }

    // Turn the properties into byte arrays, so it can be stored properly in the database.
    public byte[] ToArray()
    {
        byte[] hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(_salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(_hash, 0, hashBytes, SaltSize, HashSize);
        return hashBytes;
    }
    
    // Getters for salt and the hash.
    public byte[] Salt { get { return (byte[])_salt.Clone(); } }
    public byte[] Hash { get { return (byte[])_hash.Clone(); } }

    // Verify the password by utilizing the user-entered password and salt.
    public bool Verify(string password, byte[] salt)
    {
        byte[] test = new Rfc2898DeriveBytes(password, salt, HashIter).GetBytes(HashSize);
        for (int i = 0; i < HashSize; i++)
            if (test[i] != _hash[i])
                return false;
        return true;
    }
}