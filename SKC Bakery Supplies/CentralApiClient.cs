using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SKC_Bakery_Supplies
{
    public static class CentralApiClient
    {
        private static readonly string ApiBaseUrl = "http://100.84.79.35:7290";

        private static readonly HttpClient client = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        })
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // -----------------------------------------------------------
        // INVENTORY READS
        // -----------------------------------------------------------

        public static async Task<List<BakeryProduct>> GetAllProductsAsync()
        {
            HttpResponseMessage response = await client.GetAsync($"{ApiBaseUrl}/api/inventory");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<BakeryProduct>>(content, jsonOptions) ?? new List<BakeryProduct>();
            }

            string errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch catalog. Code: {response.StatusCode}\nDetails: {errorDetails}");
        }
    }
}