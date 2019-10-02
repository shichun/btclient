using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace bttest
{
    class HttpClientRequester
    {
        private static readonly HttpClientRequester _instance = new HttpClientRequester();
        private readonly string secretKey = "baf5877617352b1f6168f77211e12b11";

        private HttpClientRequester()
        {
        }
        public static HttpClientRequester Instance
        {
            get { return _instance; }
        }

        private HttpClient client = new HttpClient();
        private string URL = "http://localhost:8080/hhg3/service/upload";

        public void PostRequest(string dataType, string dataString)
        {
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.AcceptCharset.Add(
                new StringWithQualityHeaderValue("utf-8"));
            string dt = new DateTime().ToLongDateString();
            HttpContent content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("dataType", dataType),
                new KeyValuePair<string, string>("data", dataString),
                new KeyValuePair<string, string>("dt",dt),
                new KeyValuePair<string, string>("sign",generateSecuritySign(dataType, dt))
            });
            HttpResponseMessage response = client.PostAsync(client.BaseAddress, content).Result;
            if (response.IsSuccessStatusCode)
            {
            }
        }

        private string generateSecuritySign(string dataType, string dt)
        {
            String sign = "dataType#" + dataType + "#dt" + dt + "#secretKey" + secretKey;
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, sign);
                return hash;
            }
        }

        private string  GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}
