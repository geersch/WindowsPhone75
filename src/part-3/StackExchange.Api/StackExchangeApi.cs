using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using SharpCompress.Compressor;
using SharpCompress.Compressor.Deflate;

namespace StackExchange.Api
{
    public class StackExchangeApi
    {
        private const string ApiVersion = "1.1";
        private const string BaseUri = "http://api.stackoverflow.com/" + ApiVersion;
        private readonly string _apiKey;

        public StackExchangeApi(string apiKey)
        {
            this._apiKey = apiKey;
        }

        public int? MaxRateLimit { get; set; }
        public int? CurrentRateLimit { get; set; }

        private string ComposeUri(string path)
        {
            var uri = String.Format("{0}{1}", BaseUri, path);
            if (!String.IsNullOrWhiteSpace(this._apiKey))
            {
                var separator = uri.Contains("?") ? "&" : "?";
                uri = String.Format("{0}{1}key={2}", uri, separator, this._apiKey);
            }
            return uri;
        }

        private void ParseHeaders(WebResponse response)
        {
            if (response.Headers == null) return;

            if (response.Headers.AllKeys.Contains("X-RateLimit-Max"))
            {
                this.MaxRateLimit = Int32.Parse(response.Headers["X-RateLimit-Max"]);
            }
            if (response.Headers.AllKeys.Contains("X-RateLimit-Current"))
            {
                this.CurrentRateLimit = Int32.Parse(response.Headers["X-RateLimit-Current"]);
            }
        }

        private string ExtractJsonResponse(WebResponse response)
        {
            ParseHeaders(response);

            string json;
            using (var outStream = new MemoryStream())
            using (var zipStream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
            {
                zipStream.CopyTo(outStream);
                outStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(outStream, Encoding.UTF8))
                {
                    json = reader.ReadToEnd();
                }
            }
            return json;
        }

        private void GetStackExchangeObject<T>(string path, StackExchangeApiCallback<T> callback) 
            where T : class, new()
        {
            var requestUri = ComposeUri(path);
            GetSingleObject(requestUri, callback);
        }

        private void GetSingleObject<T>(string requestUri, StackExchangeApiCallback<T> callback) 
            where T : class, new()
        {
            var request = (HttpWebRequest) WebRequest.Create(requestUri);
            request.Method = "GET";
            request.Accept = "application/json";

            request.BeginGetResponse(
                ar =>
                    {
                        var response = (HttpWebResponse)request.EndGetResponse(ar);
                        var json = ExtractJsonResponse(response);
                        var data = ParseJson<T>(json).FirstOrDefault();
                        callback(data);
                    },
                null);
        }

        private void GetMultipleObjects<T>(string requestUri, StackExchangeApiCallback<List<T>> callback) where T : class, new()
        {
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = "GET";
            request.Accept = "application/json";

            request.BeginGetResponse(
                ar =>
                {
                    var response = (HttpWebResponse)request.EndGetResponse(ar);
                    var json = ExtractJsonResponse(response);
                    var data = ParseJson<T>(json).ToList();
                    callback(data);
                },
                null);
        }

        private static IEnumerable<T> ParseJson<T>(string json) where T : class, new()
        {
            var type = typeof(T);
            var attribute = type.GetCustomAttributes(typeof(WrapperObjectAttribute), false).SingleOrDefault() as WrapperObjectAttribute;
            if (attribute == null)
            {
                throw new InvalidOperationException(
                    String.Format("{0} type must be decorated with a WrapperObjectAttribute.", type.Name));
            }

            var jobject = JObject.Parse(json);
            var collection = JsonConvert.DeserializeObject<List<T>>(jobject[attribute.WrapperObject].ToString());
            return collection;
        }

        private void GetStackExchangeObjects<T>(string path, StackExchangeApiCallback<List<T>> callback) where T : class, new()
        {
            var requestUri = ComposeUri(path);
            GetMultipleObjects(requestUri, callback);
        }

        public delegate void StackExchangeApiCallback<T>(T data) where T : class, new();

        public void GetUser(int userId, StackExchangeApiCallback<User> callback)
        {
            GetStackExchangeObject(String.Format("/users/{0}", userId), callback);
        }

        public void GetReputationChanges(int userId, DateTime fromDate, DateTime toDate, StackExchangeApiCallback<List<ReputationChange>> callback)
        {
            var path = String.Format("/users/{0}/reputation?fromdate={1}&todate={2}", userId,
                fromDate.ToUnixTime(), toDate.ToUnixTime());

            GetStackExchangeObjects(path, callback);
        }
    }
}
