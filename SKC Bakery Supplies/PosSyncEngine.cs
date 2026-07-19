using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKC_Bakery_Supplies
{
    public class SyncOutcome
    {
        public bool Online { get; set; }
        public int Pushed { get; set; }
        public int Shortfalls { get; set; }
        public int Rejected { get; set; }
        public bool CatalogRefreshed { get; set; }
        public List<string> Warnings { get; } = new();
    }

    // Push-then-pull sync for the office POS. Push first so the pulled stock snapshot already
    // reflects the sales we just sent. Everything is idempotent server-side, so a crash
    // between push and ResolvePendingSale just means one harmless AlreadySynced next run.
    public static class PosSyncEngine
    {
        private static bool syncing;

        public static DateTime? LastSyncAt { get; private set; }
        public static bool LastSyncOnline { get; private set; }

        public static async Task<SyncOutcome> SyncAsync(string branch)
        {
            var outcome = new SyncOutcome();

            // A 60s timer tick can land while a slow manual sync is still running;
            // overlapping pushes of the same queue would be wasteful (though safe).
            if (syncing) return outcome;
            syncing = true;

            try
            {
                // ---- push ----
                var pending = PosLocalStore.GetPendingSales(branch);
                if (pending.Count > 0)
                {
                    List<SaleSyncResult> results;
                    try
                    {
                        results = await CentralApiClient.PushSalesAsync(pending);
                    }
                    catch
                    {
                        // Offline (or server unreachable): queue stays intact, try again later.
                        LastSyncOnline = false;
                        return outcome;
                    }

                    foreach (var r in results)
                    {
                        PosLocalStore.ResolvePendingSale(r.ClientSaleId,
                            r.Status == "AlreadySynced" ? "Synced" : r.Status, r.Detail);

                        switch (r.Status)
                        {
                            case "Synced":
                            case "AlreadySynced":
                                outcome.Pushed++;
                                break;
                            case "SyncedWithShortfall":
                                outcome.Pushed++;
                                outcome.Shortfalls++;
                                outcome.Warnings.Add($"Sale {r.ClientSaleId}: {r.Detail}");
                                break;
                            case "Rejected":
                                outcome.Rejected++;
                                outcome.Warnings.Add($"Sale {r.ClientSaleId} REJECTED: {r.Detail}");
                                break;
                        }
                    }
                }

                // ---- pull ----
                try
                {
                    var stock = await CentralApiClient.GetBranchInventoryAsync(branch);
                    PosLocalStore.RefreshCatalog(stock);
                    outcome.CatalogRefreshed = true;
                }
                catch
                {
                    // Push may have succeeded while the pull failed; still counts as online
                    // if anything got through.
                    LastSyncOnline = outcome.Pushed > 0;
                    outcome.Online = LastSyncOnline;
                    if (outcome.Online) LastSyncAt = DateTime.Now;
                    return outcome;
                }

                outcome.Online = true;
                LastSyncOnline = true;
                LastSyncAt = DateTime.Now;
                return outcome;
            }
            finally
            {
                syncing = false;
            }
        }
    }
}
