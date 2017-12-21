using System;
using System.Configuration;
using System.IO;

namespace Imagine.BookManager.Common
{
    public class ConfigHelper
    {

        public static readonly string PaymentBody;

        public static readonly string PaymentSubject;

        static ConfigHelper()
        {
            PaymentBody = GetValue("PaymentBody");
            PaymentSubject = GetValue("PaymentSubject");
        }

        public static string GetValue(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value))
                throw new Exception("key " + key + " or vaule is empty");
            return value;
        }

        public static string GetFileContent(string key)
        {
            string path = GetValue(key);
            if (File.Exists(path) == false)
            {
                throw new FileNotFoundException("the file " + path + " not exists");
            }
            return File.ReadAllText(path);
        }
    }
}