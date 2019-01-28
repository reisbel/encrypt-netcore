using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

//based on https://gist.github.com/davidsheardown/6781a4c45eaf85917392678d7c3993d6

namespace encrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            var key = "xQ2Zg4vlgurGji8u32hDHDQqEQv0K4e0";
            var content = "Hello World!";

            var encrypted = CryptHelper.Encrypt(content, key);
            Console.WriteLine(encrypted);

            var decrypted = CryptHelper.Decrypt(encrypted, key);
            Console.WriteLine(decrypted);

            Console.ReadLine();
        }
    }
}