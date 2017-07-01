using JSONClient;
using System;

namespace JSONClientSampleCore
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // Init JSONClient
            JsonClient.Client.ApiBaseUrl = Properties.Resources.ApiBaseUrl;
            JsonClient.Client.ApiPassword = Properties.Resources.ApiPassword;
            JsonClient.Client.ApiProjectPath = Properties.Resources.ApiProjectPath;
            JsonClient.Client.ApiTokenPath = Properties.Resources.ApiTokenPath;
            JsonClient.Client.ApiUserName = Properties.Resources.ApiUserName;

            var response = JsonClient.Client.PostRequest<string, string>(requestPath: "/api/values", httpContent: "jo").Result;



            Console.WriteLine(response);

            //var response2 = JsonClient.Client.GetRequest<List<Patient>>(
            //    requestPath: "/api/Patients").Result;

            //Console.WriteLine(response[1].FirstName);
            //Console.WriteLine(response[1].LastName);

            //var response3 = JsonClient.Client.GetRequest<List<Patient>>(
            //    requestPath: "/api/Patients").Result;

            //Console.WriteLine(response[2].FirstName);
            //Console.WriteLine(response[2].LastName);
        }
    }
}