using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SKC_Branch
{
    // Thrown when an accept fails because the ticket no longer exists (office amended/deleted it).
    public class DeliveryChangedException : Exception
    {
        public DeliveryChangedException(string message) : base(message) { }
    }

    public static class BranchApiClient
    {
        private static readonly string ApiBaseUrl = "http://100.84.79.35:7290"; // droplet
        // private static readonly string ApiBaseUrl = "http://localhost:53756";

        private static readonly HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task<List<BranchProduct>> GetAllProductsAsync()
        {
            HttpResponseMessage response = await client.GetAsync($"{ApiBaseUrl}/api/inventory");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<BranchProduct>>(content, jsonOptions) ?? new List<BranchProduct>();
            }

            string errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch catalog. Code: {response.StatusCode}\nDetails: {errorDetails}");
        }

        public static async Task<List<DeliveryTicketSummary>> GetPendingDeliveriesAsync(string branch)
        {
            HttpResponseMessage response = await client.GetAsync(
                $"{ApiBaseUrl}/api/deliveries/pending?branch={Uri.EscapeDataString(branch)}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<DeliveryTicketSummary>>(content, jsonOptions) ?? new List<DeliveryTicketSummary>();
            }

            string errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch pending deliveries. Code: {response.StatusCode}\nDetails: {errorDetails}");
        }

        public static async Task<List<DeliveryLog>> GetDeliveryDetailsAsync(string transactionId)
        {
            HttpResponseMessage response = await client.GetAsync(
                $"{ApiBaseUrl}/api/deliveries/{Uri.EscapeDataString(transactionId)}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<DeliveryLog>>(content, jsonOptions) ?? new List<DeliveryLog>();
            }

            string errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch delivery details. Code: {response.StatusCode}\nDetails: {errorDetails}");
        }

        public static async Task AcceptDeliveryAsync(string transactionId, string branch, string acceptedBy)
        {
            var response = await client.PostAsJsonAsync(
                $"{ApiBaseUrl}/api/deliveries/{Uri.EscapeDataString(transactionId)}/accept",
                new { Branch = branch, AcceptedBy = acceptedBy }, jsonOptions);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new Exception("This delivery has already been accepted.");
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // The ticket no longer exists - the office deleted or amended it after this
                // screen last refreshed. Signal that distinctly so the UI can prompt a review.
                throw new DeliveryChangedException(
                    "This delivery was changed by the office. Please review the updated list.");
            }
            if (!response.IsSuccessStatusCode)
            {
                string errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to accept delivery.\nDetails: {errorDetails}");
            }
        }

        public static async Task<List<BranchStockItem>> GetMyStockAsync(string branch)
        {
            HttpResponseMessage response = await client.GetAsync(
                $"{ApiBaseUrl}/api/inventory/branch/{Uri.EscapeDataString(branch)}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<BranchStockItem>>(content, jsonOptions) ?? new List<BranchStockItem>();
            }

            string errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch branch stock. Code: {response.StatusCode}\nDetails: {errorDetails}");
        }

        public static async Task<bool> CheckHealthAsync()
        {
            try
            {
                var response = await client.GetAsync($"{ApiBaseUrl}/health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
