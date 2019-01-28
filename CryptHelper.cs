using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace encrypt
{
     /// <summary>
    /// Crypto Helper class
    /// </summary>
    public static class CryptHelper
    {
        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly byte[] InitVectorBytes = Encoding.ASCII.GetBytes("3JblnDyu7OUCktwk");

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int Keysize = 256;

        /// <summary>
        /// Encrypt text
        /// </summary>
        /// <param name="plainText">Plain text to encrypt</param>
        /// <param name="key">Encrypt Key</param>
        /// <returns>Cipher Text</returns>
        public static string Encrypt(string plainText, string key)
        {
            //get plain text byte[]
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            //declare password
            using (var password = new PasswordDeriveBytes(key, null))
            {
                //get password byte[]
                byte[] keyBytes = password.GetBytes(Keysize / 8);
                //initialice Crypt 
                using (var symmetricKey = new RijndaelManaged())
                {
                    //assign symmetric key mode
                    symmetricKey.Mode = CipherMode.CBC;
                    //initialice encryptor
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, InitVectorBytes))
                    {
                        //inicialice memory stream
                        using (var memoryStream = new MemoryStream())
                        {
                            //initialice crypt stream using memory string and encryptor
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                //write to crypt stream
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                //flush to final
                                cryptoStream.FlushFinalBlock();
                                //get cipher text byte []
                                var cipherTextBytes = memoryStream.ToArray();
                                //convert cypher text to base 64 string and return
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decrypt text
        /// </summary>
        /// <param name="cipherText">Cipher text</param>
        /// <param name="key">Key</param>
        /// <returns>Plain text</returns>
        public static string Decrypt(string cipherText, string key)
        {
            //get cypher text bytes 
            var cipherTextBytes = Convert.FromBase64String(cipherText);
            //initialice password using key
            using (var password = new PasswordDeriveBytes(key, null))
            {
                //get password bytes
                var keyBytes = password.GetBytes(Keysize / 8);
                //initialice decrypt
                using (var symmetricKey = new RijndaelManaged())
                {
                    //assign symmetric key mode to CBC
                    symmetricKey.Mode = CipherMode.CBC;
                    //create decryptor using key bytes
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, InitVectorBytes))
                    {
                        //initialice memory stream using cipher bytes
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            //inicialice crypto stream using memory stream and decryptor 
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                //get plain text bytes 
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                //get decrupted byte count 
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                //decrypt to text plain and return
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }
    }
}