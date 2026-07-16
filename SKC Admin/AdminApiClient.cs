using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SKC_Admin
{
    public static class AdminApiClient
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
