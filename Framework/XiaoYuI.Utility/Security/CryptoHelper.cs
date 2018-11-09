using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
namespace XiaoYuI.Utility.Security
{
    /// <summary>
    /// 适用于跨平台的加密解密算法
    /// sKey=12345678
    /// </summary>
   public class CryptoHelper
   {

       #region   跨平台加解密（c#）
       private static string _key = "12345678";

        public static string Encrypt(string sourceString)
        {
            return Encrypt(sourceString, _key);
        }
        /// <summary>
        /// 对字符串进行DES加密
        /// </summary>
        /// <param name="sourceString">待加密的字符串</param>
        /// <returns>加密后的BASE64编码的字符串</returns>
        public static string Encrypt(string sourceString, string sKey)
        {
            byte[] btKey = Encoding.UTF8.GetBytes(sKey);
            byte[] btIV = Encoding.UTF8.GetBytes(sKey);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Encoding.UTF8.GetBytes(sourceString);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);
                        cs.FlushFinalBlock();
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
                catch
                {
                    throw;
                }
            }
        }

        public static string Decrypt(string pToDecrypt)
        {
            return Decrypt(pToDecrypt, _key);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位</param>
        /// <returns>已解密的字符串</returns>
        public static string Decrypt(string pToDecrypt, string sKey)
        {

            //转义特殊字符
            pToDecrypt = pToDecrypt.Replace("-", "+");
            pToDecrypt = pToDecrypt.Replace("_", "/");
            pToDecrypt = pToDecrypt.Replace("~", "=");
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }

       #endregion


        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publickey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string publickey, string content)
        {
            if (string.IsNullOrEmpty(publickey))
                throw new ArgumentNullException("publicKey");
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSADecrypt(string privatekey, string content)
        {
            if (string.IsNullOrEmpty(privatekey))
                throw new ArgumentNullException("privateKey");
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(privatekey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);

            return Encoding.UTF8.GetString(cipherbytes);
        }

       /// <summary>
       /// 获取RSAKey值
       /// </summary>
       /// <param name="KeyFileName"></param>
       /// <returns></returns>
        public static string GetRSAKey(string KeyFileName)
        {
            string KeyValue = string.Empty;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, KeyFileName);
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        KeyValue = sr.ReadToEnd();
                    }
                }
            }
            catch {
                throw;
            }
            return KeyValue;
        }


        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publickey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSAEncryptWrap(string publickey, string content)
        {
            if (string.IsNullOrEmpty(publickey))
                throw new ArgumentNullException("publicKey");
            CspParameters RSAParams = new CspParameters();
            RSAParams.Flags = CspProviderFlags.UseMachineKeyStore;
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024, RSAParams);  
            //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            #region 分段处理

            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = ByteConverter.GetBytes(content);
            int keySize = rsa.KeySize / 8;
            int bufferSize = keySize - 11;
            byte[] buffer = new byte[bufferSize];

            using (MemoryStream msIntput = new MemoryStream(dataToEncrypt))
            {
                using (MemoryStream msOutput = new MemoryStream())
                {
                    int readLen = msIntput.Read(buffer, 0, bufferSize);
                    while (readLen > 0)
                    {

                        byte[] dataToEnc = new byte[readLen];
                        Array.Copy(buffer, 0, dataToEnc, 0, readLen);

                        byte[] encData = rsa.Encrypt(dataToEnc, false);
                        msOutput.Write(encData, 0, encData.Length);
                        readLen = msIntput.Read(buffer, 0, bufferSize);

                    }
                    cipherbytes = msOutput.ToArray();    //得到加密结果
                }
            }
            #endregion
            return  Convert.ToBase64String(cipherbytes);;
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSADecryptWrap(string privatekey, string content)
        {
            if (string.IsNullOrEmpty(privatekey))
                throw new ArgumentNullException("privateKey");
            CspParameters RSAParams = new CspParameters();
            RSAParams.Flags = CspProviderFlags.UseMachineKeyStore;
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024, RSAParams);  

           // RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] source;
            rsa.FromXmlString(privatekey);
            #region 合并分段
            int keySize = rsa.KeySize / 8;
            byte[] buffer = new byte[keySize];
            byte[] dataEnc = Convert.FromBase64String(content);   //加载密文
            using (MemoryStream msIntput = new MemoryStream(dataEnc))
            {
                using (MemoryStream msOutput = new MemoryStream())
                {
                    int readLen = msIntput.Read(buffer, 0, keySize);
                    while (readLen > 0)
                    {
                        byte[] dataToDec = new byte[readLen];
                        Array.Copy(buffer, 0, dataToDec, 0, readLen);
                        byte[] decData = rsa.Decrypt(dataToDec, false);
                        msOutput.Write(decData, 0, decData.Length);
                        readLen = msIntput.Read(buffer, 0, keySize);
                    }
                    source = msOutput.ToArray();    //得到解密结果
                }
            }
            #endregion
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            return ByteConverter.GetString(source);
        }

      

       /// <summary>
       /// 
       /// </summary>
       /// <param name="source"></param>
       /// <returns></returns>
        public static string EncodeBase64(string source)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
        }
         
       
       /// <summary>
       /// 
       /// </summary>
       /// <param name="result"></param>
       /// <returns></returns>
       public static string DecodeBase64(string result)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(result));
        }

       #region 创建生成 MachineKey
       /// <summary>
       /// 
       /// </summary>
       /// <param name="decryption"></param>
       /// <param name="decryptionLength"></param>
       /// <param name="validation"></param>
       /// <param name="validationLength"></param>
       /// <returns></returns>
        public static string CreateMachineKey(string decryption, int decryptionLength, string validation, int validationLength)
        {

            string decryptionKey = CreateKey(decryptionLength);
            string validationKey = CreateKey(validationLength);
            return @"<machineKey decryption=""" + decryption
                + @""" decryptionKey=""" + decryptionKey
                + @",IsolateApps"" validation=""" + validation
                + @"""validationKey=""" + validationKey
                + @",IsolateApps""/>";

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="numBytes"></param>
        /// <returns></returns>
        private static String CreateKey(int numBytes)
        {

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            byte[] buff = new byte[numBytes];

            rng.GetBytes(buff);
            return BytesToHexString(buff);

        }


        private static String BytesToHexString(byte[] bytes)
        {

            StringBuilder hexString = new StringBuilder(64);
            for (int counter = 0; counter < bytes.Length; counter++)
            {
                hexString.Append(String.Format("{0:X2}", bytes[counter]));
            }
            return hexString.ToString();

        }
       #endregion
   }
}
