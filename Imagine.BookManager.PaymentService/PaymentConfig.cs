using System;
using System.Configuration;
using System.IO;

namespace Imagine.BookManager.PaymentService
{
    public class PaymentConfig
    {

        public static readonly string PaymentBody;

        public static readonly string PaymentSubject;

        static PaymentConfig()
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
                throw new Exception("the file " + path + " not exists");
            }
            return File.ReadAllText(path);
        }
    }
}