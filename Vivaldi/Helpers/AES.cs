

namespace Vivaldi.Helpers
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    class AES
    {

        public static byte[] EncryptAesManaged(string datos)
        {
            byte[] encrypted = null;

            try
            {
                byte[] Key = Encoding.ASCII.GetBytes(Models.Encrypt.AES.Key);
                byte[] Iv = Encoding.ASCII.GetBytes(Models.Encrypt.AES.Iv);

                // Create Aes that generates a new key and initialization vector (IV).    
                // Same key must be used in encryption and decryption    
                using (AesManaged aes = new AesManaged())
                {
                    // Encrypt string    
                    encrypted = Encrypt(datos, Key, Iv);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            return encrypted;
        }

        public static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }

        public static string DecryptAES(string cipherText)
        {
            byte[] Key = Encoding.ASCII.GetBytes(Models.Encrypt.AES.Key);
            byte[] Iv = Encoding.ASCII.GetBytes(Models.Encrypt.AES.Iv);
            byte[] b = Encoding.Default.GetBytes(cipherText);
            byte[] bytes = Convert.FromBase64String(cipherText);

            string plaintext = null;
            // Create AesManaged  
            try
            {
                using (AesManaged aes = new AesManaged())
                {
                    // Create a decryptor    
                    ICryptoTransform decryptor = aes.CreateDecryptor(Key, Iv);
                    // Create the streams used for decryption.    
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        // Create crypto stream    
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            // Read crypto stream    
                            using (StreamReader reader = new StreamReader(cs))
                                plaintext = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
            }

            return plaintext;
        }
    }
}
