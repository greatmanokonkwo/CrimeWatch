using CrimeWatch.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace CrimeWatch.Services
{
    class RestService
    {
        // http client for making requests to API
        private readonly HttpClient client;

        public RestService() 
        {
            // Initialize httpclient with a base address 
            client = new HttpClient { BaseAddress = new Uri(Constants.CrimeometerRawDataEndpoint) };

            // Add API key as Header
            client.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", Constants.CrimeometerAPIKey);
        }

        // Asynchronous Method that returns a IncidentData object if the user is able to send a url request
        public async Task<IncidentData> GetCrimeDataAsync(string uri) 
        {
            IncidentData incidentData = null;

            try
            {
                // get json data file
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    incidentData = JsonConvert.DeserializeObject<IncidentData>(content);
                }
            }
            catch (Exception ex) 
            {
                Debug.WriteLine($"\tERROR {ex.Message}");
            }

            return incidentData;
        }

        //Gets saved crime data if user cannot get updated crime data online
        public IncidentData GetCrimeData()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Database)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("CrimeWatch.CrimeData.json");
            IncidentData incidentData = null;

            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            incidentData = JsonConvert.DeserializeObject<IncidentData>(json);

            return incidentData;
        }
    }
}
