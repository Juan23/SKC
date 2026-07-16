using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SKC_Bakery_Supplies
{
    public static class CentralApiClient
    {
        private static readonly string ApiBaseUrl = "http://100.84.79.35:7290"; // droplet
        // private static readonly string ApiBaseUrl = "http://localhost:53755";

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

        public static async Task<List<BakeryProduct>> GetBranchInventoryAsync(string branch)
        {
            HttpResponseMessage response = await client.GetAsync($"{ApiBaseUrl}/api/inventory/branch/{Uri.EscapeDataString(branch)}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<BakeryProduct>>(content, jsonOptions) ?? new List<BakeryProduct>();
            }

            string errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch branch inventory. Code: {response.StatusCode}\nDetails: {errorDetails}");
        }

        public static async Task<List<BranchSaleSummary>> GetBranchSalesAsync(string branch, DateTime start, DateTime end)
        {
            HttpResponseMessage response = await client.GetAsync(
                $"{ApiBaseUrl}/api/sales?branch={Uri.EscapeDataString(branch)}&start={start:yyyy-MM-ddTHH:mm:ss}&end={end:yyyy-MM-ddTHH:mm:ss}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<BranchSaleSummary>>(content, jsonOptions) ?? new List<BranchSaleSummary>();
            }

            string errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch branch sales. Code: {response.StatusCode}\nDetails: {errorDetails}");
        }

        public static async Task<List<BranchSaleLine>> GetBranchSaleLinesAsync(string branch, string clientSaleId)
        {
            HttpResponseMessage response = await client.GetAsync(
                $"{ApiBaseUrl}/api/sales/{Uri.EscapeDataString(branch)}/{Uri.EscapeDataString(clientSaleId)}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<BranchSaleLine>>(content, jsonOptions) ?? new List<BranchSaleLine>();
            }

            string errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch sale lines. Code: {response.StatusCode}\nDetails: {errorDetails}");
        }

        public static async Task AddProductAsync(object product)
        {
            var response = await client.PostAsJsonAsync($"{ApiBaseUrl}/api/inventory", product, jsonOptions);
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                throw new Exception("Duplicate SKU");
            }
            if (!response.IsSuccessStatusCode)
            {
                string errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to add product.\nDetails: {errorDetails}");
            }
        }

        // -----------------------------------------------------------
        // WRITE OPERATIONS
        // -----------------------------------------------------------

        public static async Task SubmitPurchasesAsync(object purchases)
        {
            var response = await client.PostAsJsonAsync($"{ApiBaseUrl}/api/purchases", purchases, jsonOptions);
            if (!response.IsSuccessStatusCode)
            {
                string errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Purchase submission failed.\nCode: {response.StatusCode}\nDetails: {errorDetails}");
            }
        }

        public static async Task<List<DeliveryLog>> SubmitDeliveriesAsync(object deliveries)
        {
            var response = await client.PostAsJsonAsync($"{ApiBaseUrl}/api/deliveries", deliveries, jsonOptions);
            if (!response.IsSuccessStatusCode)
            {
                string errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Delivery submission failed (Likely insufficient inventory).\nDetails: {errorDetails}");
            }
            return await response.Content.ReadFromJsonAsync<List<DeliveryLog>>(jsonOptions) ?? new List<DeliveryLog>();
        }

        // --- HISTORY & REPORTS ---
        public static async Task<List<PurchaseTicketSummary>> GetPurchaseTicketsAsync(DateTime start, DateTime end) =>
            await client.GetFromJsonAsync<List<PurchaseTicketSummary>>($"{ApiBaseUrl}/api/purchases/tickets?start={start:yyyy-MM-dd}&end={end:yyyy-MM-dd}", jsonOptions);

        public static async Task<List<PurchaseLog>> GetPurchaseDetailsAsync(string id) =>
            await client.GetFromJsonAsync<List<PurchaseLog>>($"{ApiBaseUrl}/api/purchases/{id}", jsonOptions);

        public static async Task DeletePurchaseTicketAsync(string id)
        {
            var res = await client.DeleteAsync($"{ApiBaseUrl}/api/purchases/{id}");
            if (!res.IsSuccessStatusCode) throw new Exception(await res.Content.ReadAsStringAsync());
        }

        public static async Task<List<DeliveryTicketSummary>> GetDeliveryTicketsAsync(DateTime start, DateTime end) =>
            await client.GetFromJsonAsync<List<DeliveryTicketSummary>>($"{ApiBaseUrl}/api/deliveries/tickets?start={start:yyyy-MM-dd}&end={end:yyyy-MM-dd}", jsonOptions);

        public static async Task<List<DeliveryLog>> GetDeliveryDetailsAsync(string id) =>
            await client.GetFromJsonAsync<List<DeliveryLog>>($"{ApiBaseUrl}/api/deliveries/{id}", jsonOptions);

        public static async Task DeleteDeliveryTicketAsync(string id)
        {
            var res = await client.DeleteAsync($"{ApiBaseUrl}/api/deliveries/{id}");
            if (!res.IsSuccessStatusCode) throw new Exception(ExtractProblemDetail(await res.Content.ReadAsStringAsync()));
        }

        // The API returns errors as RFC 9110 ProblemDetails JSON (via Results.Problem). Pull out
        // the human-readable "detail" so callers can show it plainly instead of a raw JSON blob.
        private static string ExtractProblemDetail(string body)
        {
            if (string.IsNullOrWhiteSpace(body)) return "Request failed.";
            try
            {
                using var doc = JsonDocument.Parse(body);
                if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                    doc.RootElement.TryGetProperty("detail", out var detail) &&
                    detail.ValueKind == JsonValueKind.String)
                {
                    var text = detail.GetString();
                    if (!string.IsNullOrWhiteSpace(text)) return text;
                }
            }
            catch (JsonException) { /* not JSON - fall through to raw body */ }
            return body;
        }

        public static async Task<List<DailyDeliveryPrintItem>> GetDailyDeliveryConsolidationAsync(DateTime date) =>
            await client.GetFromJsonAsync<List<DailyDeliveryPrintItem>>($"{ApiBaseUrl}/api/deliveries/daily?targetDate={date:yyyy-MM-dd}", jsonOptions);

        // --- PRODUCT MANAGEMENT ---
        public static async Task UpdateProductAsync(string sku, string brand, string baseName)
        {
            var response = await client.PutAsJsonAsync($"{ApiBaseUrl}/api/inventory/{sku}", new { Brand = brand, BaseName = baseName }, jsonOptions);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to update product.\nDetails: {await response.Content.ReadAsStringAsync()}");
        }

        public static async Task DeactivateProductAsync(string sku)
        {
            var response = await client.PatchAsync($"{ApiBaseUrl}/api/inventory/{sku}/deactivate", null);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to discontinue product.\nDetails: {await response.Content.ReadAsStringAsync()}");
        }

        // Reconciles system stock with a physical count. unitCost is only used if the count
        // is HIGHER than system stock (found stock needs a cost basis); pass null to let the
        // server default to the SKU's last purchase cost.
        public static async Task AdjustInventoryAsync(string sku, int newCount, decimal? unitCost, string reason, string branch = "Office")
        {
            var response = await client.PostAsJsonAsync($"{ApiBaseUrl}/api/inventory/{sku}/adjust",
                new { NewCount = newCount, UnitCost = unitCost, Reason = reason, Branch = branch }, jsonOptions);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to adjust stock.\nDetails: {await response.Content.ReadAsStringAsync()}");
        }

        public static async Task<List<InventoryAdjustment>> GetInventoryAdjustmentsAsync(DateTime start, DateTime end, string branch = null)
        {
            string url = $"{ApiBaseUrl}/api/inventory/adjustments?start={start:yyyy-MM-dd}&end={end:yyyy-MM-dd}";
            if (!string.IsNullOrWhiteSpace(branch))
                url += $"&branch={Uri.EscapeDataString(branch)}";
            return await client.GetFromJsonAsync<List<InventoryAdjustment>>(url, jsonOptions);
        }

        // --- CONNECTIVITY ---
        public static async Task CheckHealthAsync()
        {
            var response = await client.GetAsync($"{ApiBaseUrl}/health");
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Server responded with {response.StatusCode}");
        }
    }
}