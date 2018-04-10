using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using Carpa.Web.Script;
using System.Security;

namespace Test001
{
    public static class Utils
    {
         private const string HashAlgorithm = "SHA1";
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="password">原始密码</param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string SetPasswordHashed(string password, out string salt)
        {
            var saltBytes = new byte[0x10];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(saltBytes);
            }

            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] combinedBytes = saltBytes.Concat(passwordBytes).ToArray();

            byte[] hashBytes;
            using (HashAlgorithm hashAlgorithm = System.Security.Cryptography.HashAlgorithm.Create(HashAlgorithm))
            {
                if (hashAlgorithm == null)
                {
                    throw new SecurityException("创建hash实例失败");
                }
                hashBytes = hashAlgorithm.ComputeHash(combinedBytes);
            }

            salt = Convert.ToBase64String(saltBytes);
            return Convert.ToBase64String(hashBytes);
        }
        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="userPass">数据库中密码</param>
        /// <param name="passSalt">数据库中密码加密</param>
        /// <param name="password">用户输入密码</param>
        /// <returns></returns>
        public static bool ValidatePasswordHashed(string userPass, string passSalt, string password)
        {
            byte[] saltBytes = Convert.FromBase64String(passSalt);

            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);

            byte[] combinedBytes = saltBytes.Concat(passwordBytes).ToArray();

            byte[] hashBytes;
            using (HashAlgorithm hashAlgorithm = System.Security.Cryptography.HashAlgorithm.Create(HashAlgorithm))
            {
                if (hashAlgorithm == null)
                {
                    throw new SecurityException("创建hash实例失败");
                }
                hashBytes = hashAlgorithm.ComputeHash(combinedBytes);
            }

            return userPass == Convert.ToBase64String(hashBytes);
        }


    }
}