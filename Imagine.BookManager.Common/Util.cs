using System;
using System.IO;
using System.Security.Cryptography;

namespace Imagine.BookManager.Common
{
    public class Util
    {
        public static string CreateOrderRef()
        {
            return DateTime.Now.Ticks + new Random().Next(100000, 999999).ToString();
        }


        public static string CreateMd5(string value)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = File.ReadAllBytes(value);
            File.OpenRead(value);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();
            string ret = "";
            foreach (byte t in bytes)
            {
                ret += Convert.ToString(t, 16).PadLeft(2, '0');
            }
            return ret.PadLeft(32, '0');
        }
    }
}
