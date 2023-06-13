using Saas.Core.Infrastructure.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Core.Infrastructure.Utilities
{
    /// <summary>
    /// AES对称加密
    /// </summary>
    public class AESEncryption
    {
        /// <summary>
        /// 生成随机的16位key
        /// </summary>
        /// <returns></returns>
        public static string GenerateKey()
        {
            string key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);
            return key;
        }


        /// <summary>  
        /// AES加密字符串  
        /// </summary>  
        /// <param name="encryptString">待加密的字符串</param>  
        /// <param name="key">加密密钥,要求为16位</param>  
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>  
        public static string EncryptAES(string encryptString, string key)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(key);
                //用于对称算法的初始化向量（默认值）。  
                byte[] rgbIV = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                Aes dCSP = Aes.Create();
                dCSP.Mode = CipherMode.CBC;
                dCSP.Padding = PaddingMode.PKCS7;
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>  
        /// AES解密字符串  
        /// </summary>  
        /// <param name="decryptString">待解密的字符串</param>  
        /// <param name="key">解密密钥，要求16位</param>  
        /// <returns></returns>  
        public static string DecryptAES(string decryptString, string key)
        {
            try
            {
                //用于对称算法的初始化向量（默认值）  
                byte[] Keys = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
                byte[] rgbKey = Encoding.UTF8.GetBytes(key);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                Aes DCSP = Aes.Create();
                DCSP.Mode = CipherMode.CBC;
                DCSP.Padding = PaddingMode.PKCS7;
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}
