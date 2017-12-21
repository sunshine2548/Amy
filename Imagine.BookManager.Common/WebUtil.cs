using System;
using System.IO;
using System.Net;
using System.Text;

namespace Imagine.BookManager.Common
{
    public class WebUtil
    {
        public static string PostRequest(string url, string paramter, string contentType = "text/xml")
        {
            string result = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contentType;
                byte[] data = Encoding.UTF8.GetBytes(paramter);
                request.ContentLength = data.Length;
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                if (responseStream == null)
                    return result;
                StreamReader sr = new StreamReader(result, Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (WebException e)
            {
                throw new Exception(e.ToString());
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                response?.Close();
                request?.Abort();
            }
            return result;
        }
    }
}