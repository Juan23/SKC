using System.Net.Http.Json;
using System.Text.Json;

namespace SKC_Admin_CLI
{
    // Raised when the server can't be reached at all - no connection, no route, or a timed-out
    // request. Distinct from an HTTP error response (which means we DID reach the server): this is
    // the "you're offline" case and its Message is safe to show verbatim.
    public class OfflineException : Exception
    {
        public OfflineException(Exception inner)
            : base("Can't reach the server - this device looks offline. Nothing was changed; "
                 + "check the connection and try again.", inner) { }
    }

    // Translates transport failures (connection refused, DNS/route failure) and request timeouts
    // into one friendly OfflineException, so every endpoint method reports the same clean message
    // instead of a raw HttpRequestException/TaskCanceledException.
    internal class OfflineTranslatingHandler : DelegatingHandler
    {
        public OfflineTranslatingHandler(HttpMessageHandler inner) : base(inner) { }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (HttpRequestException ex) { throw new OfflineException(ex); }
            catch (OperationCanceledException ex) { throw new OfflineException(ex); }
        }
    }

    // Same shape as SKC Admin/AdminApiClient.cs (no-shared-code convention): static HttpClient,
    // hardcoded droplet URL with a commented-out localhost line, one method per endpoint that
    // throws with the response body on a non-success status.
    public static class AdminApiClient
    {
        private static readonly string ApiBaseUrl = "http://100.84.79.35:7290"; // droplet
        // private static readonly string ApiBaseUrl = "http://localhost:53756";

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

        // includeInactive is off by default so this matches what every other client sees; the CLI
        // asks for the full list only where deactivated recipes actually matter (list --all,
        // activate, and the import's name-collision check).
        public static async Task<List<Recipe>> GetRecipesAsync(bool includeInactive = false)
        {
            string url = $"{ApiBaseUrl}/api/recipes" + (includeInactive ? "?includeInactive=true" : "");
            HttpResponseMessage response = await client.GetAsync(url);

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

        public static async Task SetRecipeActiveAsync(int recipeId, bool active)
        {
            string verb = active ? "activate" : "deactivate";
            var response = await client.PatchAsync($"{ApiBaseUrl}/api/recipes/{recipeId}/{verb}", null);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to {verb} recipe.\nDetails: {await response.Content.ReadAsStringAsync()}");
        }

        public static async Task<List<SaleLineExport>> GetSalesLinesAsync(string branch, DateTime start, DateTime end)
        {
            string url = $"{ApiBaseUrl}/api/sales/lines"
                + $"?branch={Uri.EscapeDataString(branch)}"
                + $"&start={Uri.EscapeDataString(start.ToString("o"))}"
                + $"&end={Uri.EscapeDataString(end.ToString("o"))}";
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<SaleLineExport>>(content, jsonOptions) ?? new List<SaleLineExport>();
            }

            string errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch sales lines. Code: {response.StatusCode}\nDetails: {errorDetails}");
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
