using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SKC_Admin
{
    // Raised when the server can't be reached at all - no connection, no route, or a timed-out
    // request. Distinct from an HTTP error response (which means we DID reach the server): this is
    // the "you're offline" case and its Message is safe to show verbatim. Nothing the caller had
    // entered is touched - the screen just catches this and reports it.
    public class OfflineException : Exception
    {
        public OfflineException(Exception inner)
            : base("Can't reach the server - this device looks offline. Nothing you entered was lost; "
                 + "check the connection and try again.", inner) { }
    }

    // Translates transport failures (connection refused, DNS/route failure) and request timeouts
    // into one friendly OfflineException, so every endpoint method reports the same clean message
    // instead of a raw HttpRequestException/TaskCanceledException. No caller passes a cancellation
    // token, so a cancellation observed here is always the HttpClient timeout elapsing.
    internal class OfflineTranslatingHandler : DelegatingHandler
    {
        public OfflineTranslatingHandler(HttpMessageHandler inner) : base(inner) { }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (HttpRequestException ex) { throw new OfflineException(ex); }
            catch (OperationCanceledException ex) { throw new OfflineException(ex); }
        }
    }

    public static class AdminApiClient
    {
        private static readonly string ApiBaseUrl = "http://100.84.79.35:7290"; // droplet
        // private static readonly string ApiBaseUrl = "http://localhost:53756";

        // Wrapped in OfflineTranslatingHandler so a connection failure or a 10s timeout surfaces as
        // a friendly OfflineException instead of a raw exception. 10s (down from 30) because the
        // payloads are tiny - if we can't reach the droplet we know almost immediately.
        private static readonly HttpClient client = new HttpClient(new OfflineTranslatingHandler(new HttpClientHandler()))
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task<List<AdminProduct>> GetAllProductsAsync()
        {
            HttpResponseMessage response = await client.GetAsync($"{ApiBaseUrl}/api/inventory");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<AdminProduct>>(content, jsonOptions) ?? new List<AdminProduct>();
            }

            string errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch catalog. Code: {response.StatusCode}\nDetails: {errorDetails}");
        }

        public static async Task SetClassificationAsync(string sku, string category, string? uom, decimal packMultiplier)
        {
            var response = await client.PutAsJsonAsync(
                $"{ApiBaseUrl}/api/inventory/{Uri.EscapeDataString(sku)}/classification",
                new { Category = category, Uom = uom, PackMultiplier = packMultiplier }, jsonOptions);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to update classification.\nDetails: {await response.Content.ReadAsStringAsync()}");
        }

        public static async Task SetPriceAsync(string sku, decimal price)
        {
            var response = await client.PutAsJsonAsync(
                $"{ApiBaseUrl}/api/inventory/{Uri.EscapeDataString(sku)}/price",
                new { Price = price }, jsonOptions);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to update price.\nDetails: {await response.Content.ReadAsStringAsync()}");
        }

        public static async Task<List<Recipe>> GetRecipesAsync()
        {
            HttpResponseMessage response = await client.GetAsync($"{ApiBaseUrl}/api/recipes");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Recipe>>(content, jsonOptions) ?? new List<Recipe>();
            }

            string errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch recipes. Code: {response.StatusCode}\nDetails: {errorDetails}");
        }

        public static async Task CreateRecipeAsync(Recipe recipe)
        {
            var response = await client.PostAsJsonAsync($"{ApiBaseUrl}/api/recipes", recipe, jsonOptions);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to create recipe.\nDetails: {await response.Content.ReadAsStringAsync()}");
        }

        public static async Task UpdateRecipeAsync(int recipeId, Recipe recipe)
        {
            var response = await client.PutAsJsonAsync($"{ApiBaseUrl}/api/recipes/{recipeId}", recipe, jsonOptions);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to update recipe.\nDetails: {await response.Content.ReadAsStringAsync()}");
        }

        public static async Task DeactivateRecipeAsync(int recipeId)
        {
            var response = await client.PatchAsync($"{ApiBaseUrl}/api/recipes/{recipeId}/deactivate", null);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to deactivate recipe.\nDetails: {await response.Content.ReadAsStringAsync()}");
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
