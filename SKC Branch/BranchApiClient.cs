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

    // Thrown when an accept returns 409: the ticket is already Accepted, so this branch's stock was
    // already credited (a double-click, a retry after a lost response, or another device on this
    // branch got there first). The UI treats it as success, not an error - see frmMain.
    public class AlreadyAcceptedException : Exception
    {
        public AlreadyAcceptedException(string message) : base(message) { }
    }

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

    // Thrown when the server WAS reached but returned a non-success HTTP status for a POS sync
    // push (e.g. a 403 IP-gate rejection or a 400 malformed batch). Distinct from OfflineException
    // (a transport failure): the sync engine must NOT treat this as "offline, retry silently" -
    // it surfaces the status so a persistent gate/rejection can't masquerade as a stuck OFFLINE
    // badge while sales pile up unsent. See PosSyncEngine / frmPos status handling.
    public class PosSyncHttpException : Exception
    {
        public System.Net.HttpStatusCode StatusCode { get; }
        // A short one-liner safe for the status bar, e.g. "HTTP 403 Forbidden".
        public string ShortMessage { get; }

        public PosSyncHttpException(System.Net.HttpStatusCode statusCode, string body)
            : base($"Server returned {(int)statusCode} {statusCode}. {body}")
        {
            StatusCode = statusCode;
            ShortMessage = $"HTTP {(int)statusCode} {statusCode}";
        }
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

    public static class BranchApiClient
    {
        private static readonly string ApiBaseUrl = "http://100.84.79.35:7290"; // droplet
        // private static readonly string ApiBaseUrl = "http://localhost:53756";

        // Wrapped in OfflineTranslatingHandler so a connection failure or a 10s timeout surfaces as
        // a friendly OfflineException instead of a raw exception. 10s (down from 30) because the
        // payloads are tiny - if we can't reach the droplet we know almost immediately, and staff
        // shouldn't stare at a frozen button for half a minute.
        private static readonly HttpClient client = new HttpClient(new OfflineTranslatingHandler(new HttpClientHandler()))
        {
            Timeout = TimeSpan.FromSeconds(10)
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
                // Already accepted = this branch already has the stock. Not a failure - signal it
                // distinctly so the UI reports success and just refreshes the list off the screen.
                throw new AlreadyAcceptedException(
                    "This delivery was already accepted - the items are already in your stock.");
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

        public static async Task<ProductionResult> SubmitProductionAsync(string branch, int recipeId, string staffName,
            decimal batchMultiplier, int outputQty, string transactionId)
        {
            var response = await client.PostAsJsonAsync($"{ApiBaseUrl}/api/production",
                new
                {
                    Branch = branch,
                    RecipeId = recipeId,
                    StaffName = staffName,
                    BatchMultiplier = batchMultiplier,
                    OutputQty = outputQty,
                    TransactionId = transactionId
                }, jsonOptions);

            if (!response.IsSuccessStatusCode)
            {
                string errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Production failed (likely insufficient stock).\nDetails: {errorDetails}");
            }

            return await response.Content.ReadFromJsonAsync<ProductionResult>(jsonOptions) ?? new ProductionResult();
        }

        public static async Task<List<ProductionBatch>> GetProductionHistoryAsync(string branch)
        {
            HttpResponseMessage response = await client.GetAsync(
                $"{ApiBaseUrl}/api/production?branch={Uri.EscapeDataString(branch)}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ProductionBatch>>(content, jsonOptions) ?? new List<ProductionBatch>();
            }

            string errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch production history. Code: {response.StatusCode}\nDetails: {errorDetails}");
        }

        // Pushes queued POS sales. The server is idempotent by ClientSaleId, so re-pushing
        // after a lost response is safe - AlreadySynced comes back instead of a double-deduct.
        public static async Task<List<SaleSyncResult>> PushSalesAsync(List<PosSaleDto> sales)
        {
            var response = await client.PostAsJsonAsync($"{ApiBaseUrl}/api/sales", sales, jsonOptions);

            if (!response.IsSuccessStatusCode)
            {
                // A reached-but-rejected batch (403/400/500) is NOT the offline case - throw the
                // typed exception so PosSyncEngine can flag it distinctly instead of retrying
                // forever under an OFFLINE badge.
                string errorDetails = await response.Content.ReadAsStringAsync();
                throw new PosSyncHttpException(response.StatusCode, errorDetails);
            }

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<SaleSyncResult>>(content, jsonOptions) ?? new List<SaleSyncResult>();
        }

        // Branch-side sales history (same endpoints the office Branch Sales Report uses). The caller
        // passes an inclusive end (e.g. end-of-day) since /api/sales filters sold_at <= end.
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
            throw new Exception($"Failed to fetch sales history. Code: {response.StatusCode}\nDetails: {errorDetails}");
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
            throw new Exception($"Failed to fetch sale detail. Code: {response.StatusCode}\nDetails: {errorDetails}");
        }

        // Flat line-level sales for a date range (one row per item per sale), for the end-of-day
        // CSV export. One call for the whole range - the per-sale detail endpoint above would need
        // one round trip per sale. Same inclusive-end convention as GetBranchSalesAsync.
        public static async Task<List<BranchSaleLineExport>> GetBranchSaleLinesRangeAsync(
            string branch, DateTime start, DateTime end)
        {
            HttpResponseMessage response = await client.GetAsync(
                $"{ApiBaseUrl}/api/sales/lines?branch={Uri.EscapeDataString(branch)}&start={start:yyyy-MM-ddTHH:mm:ss}&end={end:yyyy-MM-ddTHH:mm:ss}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<BranchSaleLineExport>>(content, jsonOptions) ?? new List<BranchSaleLineExport>();
            }

            string errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch sale lines. Code: {response.StatusCode}\nDetails: {errorDetails}");
        }

        // Voids a completed (already-synced) sale. The server restocks what it consumed and flags
        // the sale; idempotent, so a retry is safe. Online-only - throws if unreachable.
        public static async Task VoidSaleAsync(string branch, string clientSaleId, string voidedBy)
        {
            var response = await client.PostAsJsonAsync(
                $"{ApiBaseUrl}/api/sales/{Uri.EscapeDataString(branch)}/{Uri.EscapeDataString(clientSaleId)}/void",
                new { VoidedBy = voidedBy }, jsonOptions);

            if (!response.IsSuccessStatusCode)
            {
                string errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to void sale. Code: {response.StatusCode}\nDetails: {errorDetails}");
            }
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
