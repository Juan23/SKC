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
        // Target your central server on the secure Tailscale IP over plain HTTP (fully encrypted by Tailscale)
        private static readonly string ApiBaseUrl = "http://100.84.79.35:7290";

        private static readonly HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        /// <summary>
        /// Orchestrates a master synchronization pass between this branch's local SQLite ledger and Central PostgreSQL.
        /// </summary>
        public static async Task<string> PerformMasterSync(string branchName)
        {
            try
            {
                var unsyncedPurchases = BakeryDatabaseManager.GetUnsyncedPurchases();
                var unsyncedDeliveries = BakeryDatabaseManager.GetUnsyncedDeliveries();
                var unsyncedLots = BakeryDatabaseManager.GetUnsyncedInventoryLots();

                // Fetch the master catalog from SQLite so we can sync it to PostgreSQL
                var inventoryItems = BakeryDatabaseManager.GetAllProducts();

                if (!unsyncedPurchases.Any() && !unsyncedDeliveries.Any() && !unsyncedLots.Any())
                    return "Database is already fully synchronized with Central.";

                var payload = new
                {
                    BranchName = branchName,
                    // Send the catalog to satisfy PostgreSQL Foreign Key constraints
                    Inventory = inventoryItems.Select(i => new { i.SKU, i.Brand, i.BaseName, i.Price, i.IsActive }).ToList(),
                    Purchases = unsyncedPurchases.Select(p => new { p.Id, p.TransactionId, p.Date, p.SKU, p.Qty, p.UnitCost, p.Supplier }).ToList(),
                    Deliveries = unsyncedDeliveries.Select(d => new { d.Id, d.TransactionId, d.Date, d.SKU, d.Qty, d.ToBranch, d.TotalLineCost, d.Requester, d.Reason }).ToList(),
                    InventoryLots = unsyncedLots.Select(l => new { l.LotId, l.SKU, l.DateReceived, l.OriginalQty, l.RemainingQty, l.UnitCost }).ToList()
                };

                string jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{ApiBaseUrl}/api/sync/master", content);

                if (response.IsSuccessStatusCode)
                {
                    var syncResult = JsonSerializer.Deserialize<SyncResponseDto>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (syncResult != null && syncResult.Success)
                    {
                        BakeryDatabaseManager.MarkPurchasesAsSynced(syncResult.SyncedPurchaseIds);
                        BakeryDatabaseManager.MarkDeliveriesAsSyncedById(syncResult.SyncedDeliveryIds);
                        BakeryDatabaseManager.MarkInventoryLotsAsSynced(syncResult.SyncedLotIds);

                        int total = (syncResult.SyncedPurchaseIds?.Count ?? 0) + (syncResult.SyncedDeliveryIds?.Count ?? 0) + (syncResult.SyncedLotIds?.Count ?? 0);
                        return $"Sync Success! Transported {total} records safely to Central server.";
                    }
                    return "Sync aborted: Central server processed the payload but returned a failure status.";
                }

                // Force the MessageBox to show us the EXACT server crash details
                string errorDetails = await response.Content.ReadAsStringAsync();
                return $"Server Rejected Sync request.\nCode: {response.StatusCode}\n\nDetails:\n{errorDetails}";
            }
            catch (Exception ex)
            {
                return $"Sync execution failed: {ex.Message}";
            }
        }
    }

    // Helper classes for deserialization matching server DTOs
    public class SyncResponseDto
    {
        public bool Success { get; set; }
        public List<string> SyncedInventorySKUs { get; set; }
        public List<int> SyncedPurchaseIds { get; set; }
        public List<int> SyncedDeliveryIds { get; set; }
        public List<int> SyncedLotIds { get; set; }
    }
}