using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace SKC_Bakery_Supplies
{
    public static class NetworkSyncManager
    {
        // Your Tailscale IP and API Port
        private static readonly string ApiBaseUrl = "https://100.108.218.24:7290";

        // The HTTP Client with a bypass for the local dev SSL certificate
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        });

        public static async Task<string> SyncDeliveriesToServer(string branchName)
        {
            try
            {
                // 1. Grab unsynced data
                var unsynced = BakeryDatabaseManager.GetUnsyncedDeliveries();

                if (unsynced == null || unsynced.Count == 0)
                {
                    return "Everything is already up to date.";
                }

                // 2. Format it to match the Server's exact JSON expectation
                var payload = new
                {
                    BranchName = branchName,
                    Deliveries = unsynced.Select(d => new
                    {
                        TransactionId = d.TransactionId,
                        Date = d.Date,
                        SKU = d.SKU,
                        Qty = d.Qty,
                        TotalLineCost = d.TotalLineCost
                    }).ToList()
                };

                string jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // 3. Fire over Tailscale
                HttpResponseMessage response = await client.PostAsync($"{ApiBaseUrl}/api/sync/deliveries", content);

                if (response.IsSuccessStatusCode)
                {
                    // 4. Only mark as synced if the server replied with a 200 OK
                    var transactionIds = unsynced.Select(x => x.TransactionId).Distinct().ToList();
                    BakeryDatabaseManager.MarkDeliveriesAsSynced(transactionIds);

                    return $"Success! Synced {unsynced.Count} delivery lines to the central server.";
                }
                else
                {
                    return $"Server rejected the sync. Status: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return $"Connection failed: {ex.Message}";
            }
        }
    }
}