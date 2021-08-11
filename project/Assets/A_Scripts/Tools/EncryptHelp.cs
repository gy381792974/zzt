using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EazyGF
{
    /// <summary>
    /// 加密类
    /// </summary>
    public class EncryptHelp
    {
        /// <summary>
        /// AES加密 密码32位
        /// </summary>
        /// <param name="array">要加密的 byte[] 数组</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] AES_Encrypt(byte[] array, string key)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(array, 0, array.Length);

            return resultArray;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="array">要解密的 byte[] 数组</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] AES_Decrypt(byte[] array, string key)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(array, 0, array.Length);

            return resultArray;
        }

        /// <summary>
        /// 异或加密
        /// </summary>
        /// <param name="targetData"></param>
        /// <param name="Key"></param>
        public static void SimpleEncypt(ref byte[] targetData, int Key)
        {
            int dataLength = targetData.Length;
            for (int i = 0; i < dataLength; ++i)
            {
                targetData[i] = (byte) (targetData[i] ^ Key);
            }
        }
        
        /// <summary>
        /// 获取随机的字节数组，参数决定是否是随机，false表示不随机，取固定长度32位
        /// </summary>
        /// <param name="IsRandomLenth"></param>
        /// <returns></returns>
        public static byte[] GetRandomByte()
        {
            Random randomNum = new Random();
            byte[] randomBytes = new byte[randomNum.Next(30, 60)];
            randomNum.NextBytes(randomBytes);
            return randomBytes;
        }

        public static string GetMD5HashFromFile(string fileName)
        {
            using (var file = new FileStream(fileName, FileMode.Open))
            {
                var md5 = new MD5CryptoServiceProvider();
                var bytes = md5.ComputeHash(file);
                file.Close();
                var sb = new StringBuilder(bytes.Length);
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
                file.Close();
                file.Dispose();
                md5 = null;
                return sb.ToString();
            }
        }
    }
}