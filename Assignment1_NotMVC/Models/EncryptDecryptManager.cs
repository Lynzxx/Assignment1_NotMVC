using System.Security.Cryptography;

namespace Assignment1_NotMVC.Models
{
    public class EncryptDecryptManager
    {
        private readonly static string key="ashproghelpdotnotmania2022key123";
        public static string Encrypt(string text)
        {
            //RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] iv = new byte[16];
            //Fills array of bytes with a cryptographically strong sequence of random values.
            //rng.GetBytes(iv);
            //string Iv = Convert.ToBase64String(iv);

            byte[] array;
            using(Aes aes = Aes.Create())
            {
                aes.Key = System.Text.Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform encryptor=aes.CreateEncryptor(aes.Key,aes.IV);
                using(MemoryStream ms=new MemoryStream())
                {
                    using(CryptoStream cryptoStream=new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamwriter=new StreamWriter(cryptoStream))
                        {
                            streamwriter.Write(text);
                        }
                        array= ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }

        public static string Decrypt(string text)
        {
     
            byte[] iv = new byte[16];
            byte[] buffer=Convert.FromBase64String(text);
            using (Aes aes = Aes.Create())
            {
                aes.Key = System.Text.Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                  
                    }
                }
            }
     
        }
    }
}
