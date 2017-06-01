using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JSONClient
{
    /// <summary>
    /// JsonClient
    /// </summary>
    public class JsonClient
    {
        /// <summary>
        /// Lazy initalization
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<JsonClient> _client = new Lazy<JsonClient>(CreateJsonClient);
        /// <summary>
        /// Returns the JSON Client
        /// </summary>
        public static JsonClient Client => _client.Value;

        /// <summary>
        /// URL to API
        /// </summary>
        public string ApiBaseUrl { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string ApiUserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string ApiPassword { get; set; }

        /// <summary>
        /// Project Path
        /// </summary>
        public string ApiProjectPath { get; set; }

        /// <summary>
        /// Token Path
        /// </summary>
        public string ApiTokenPath { get; set; }

        /// <summary>
        /// Temporary Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Last time Token was renewed
        /// </summary>
        public DateTime TimeTokenAccessed { get; set; }

        /// <summary>
        /// Constructor. Initializes Client with default values
        /// </summary>
        private JsonClient()
        {
            Console.WriteLine(@"New Client created");
        }


        /// <summary>
        /// Method for Lazy instanciation
        /// </summary>
        /// <returns></returns>
        private static JsonClient CreateJsonClient()
        {
            return new JsonClient();
        }

        /// <summary>
        /// If Token is out of date a new Token is automaticly renewed
        /// </summary>
        private void CheckTimeforToken()
        {
            if ((DateTime.Now - TimeTokenAccessed).Days <= 12) return;
            var result = GetApiToken().Result;
            Token = result;
            TimeTokenAccessed = DateTime.Now;
        }

        /// <summary>
        /// Adding Headerinformation + token
        /// </summary>
        /// <param name="client">HttpClient</param>
        private void SetupClient(HttpClient client)
        {
            client.BaseAddress = new Uri(ApiBaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
        }

        /// <summary>
        /// Consumes Token API async and returns Token to Access WebAPI
        /// </summary>
        /// <returns>string</returns>
        private async Task<string> GetApiToken()
        {
            using (var client = new HttpClient())
            {
                //setup client
                client.BaseAddress = new Uri(ApiBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //setup login data
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", ApiUserName),
                    new KeyValuePair<string, string>("password", ApiPassword),
                });

                //send request
                var responseMessage = await client.PostAsync(ApiProjectPath + ApiTokenPath, formContent).ConfigureAwait(false);

                //get access token from response body
                var responseJson = await responseMessage.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseJson);
                return jObject.GetValue("access_token").ToString();
            }
        }

        /// <summary>
        /// Get Request to WebAPI via Token Auth.
        /// </summary>
        /// <code>
        /// JsonClient.Client.GetRequest&lt;List&lt;Patient&gt;&gt;(requestPath: "/api/Patients").Result;
        /// </code>
        /// <typeparam name="T">Generic Type of request</typeparam>
        /// <param name="requestPath">string</param>
        /// <returns>requested type</returns>
        public async Task<T> GetRequest<T>(string requestPath)
        {
            CheckTimeforToken();

            using (var client = new HttpClient())
            {
                //setup client
                SetupClient(client);

                //make request
                var response = await client.GetAsync(ApiProjectPath + requestPath).ConfigureAwait(false);
                var responseString = await response.Content.ReadAsStringAsync();
                var responseType = JsonConvert.DeserializeObject<T>(responseString);
                return responseType;
            }
        }

        /// <summary>
        /// Post Request to WebAPI via Token Auth
        /// </summary>
        /// <code>
        /// JsonClient.Client.PostRequest&lt;Patient,Patient&gt;(requestPath: "/api/Patients", httpContent: patient).Result;
        /// </code>
        /// <typeparam name="T">Generic Type insert</typeparam>
        /// <typeparam name="TR">Generic Type output</typeparam>
        /// <param name="requestPath">string</param>
        /// <param name="httpContent">T</param>
        /// <returns></returns>
        public async Task<TR> PostRequest<T, TR>(string requestPath, T httpContent)
        {
            CheckTimeforToken();

            using (var client = new HttpClient())
            {
                //setup client
                SetupClient(client);

                var jsonObject = JsonConvert.SerializeObject(httpContent, Formatting.Indented);
                var stringHttpContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(ApiProjectPath + requestPath, stringHttpContent).ConfigureAwait(false);

                if (typeof(TR) == typeof(HttpResponseMessage))
                {
                    return (TR)(object)response;
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var responseType = JsonConvert.DeserializeObject<TR>(responseString);

                return responseType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <code>
        /// JsonClient.Client.PutRequest&lt;Patient&gt;(requestPath: "/api/Patients", httpContent: patient).Result;
        /// </code>
        /// <typeparam name="T">Generic type inserted</typeparam>
        /// <param name="requestPath">string</param>
        /// <param name="httpContent">T</param>
        /// <returns></returns>
        public async Task<string> PutRequest<T>(string requestPath, T httpContent)
        {
            CheckTimeforToken();

            using (var client = new HttpClient())
            {
                //setup client
                SetupClient(client);

                var jsonObject = JsonConvert.SerializeObject(httpContent, Formatting.Indented);
                var stringHttpContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(ApiProjectPath + requestPath, stringHttpContent).ConfigureAwait(false);

                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
        }

        /// <summary>
        /// Delete Request to WebAPI via Token Auth.
        /// ID in RequestPath required!
        /// </summary>
        /// <code>
        /// JsonClient.Client.DeleteRequest&lt;Patient, string&gt;(requestPath: "/api/Patients", httpContent: patient).Result;
        /// </code>
        /// <typeparam name="T">Generic type inserted</typeparam>
        /// <typeparam name="TR">Generic type output</typeparam>
        /// <param name="requestPath">string</param>
        /// <param name="httpContent">T</param>
        /// <returns></returns>
        public async Task<TR> DeleteRequest<T, TR>(string requestPath, T httpContent)
        {
            CheckTimeforToken();

            using (var client = new HttpClient())
            {
                //setup client
                SetupClient(client);

                //make request
                var response = await client.DeleteAsync(ApiProjectPath + requestPath).ConfigureAwait(false);

                if (typeof(TR) == typeof(HttpResponseMessage))
                {
                    return (TR)(object)response;
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var responseType = JsonConvert.DeserializeObject<TR>(responseString);

                return responseType;
            }
        }
    }
}
