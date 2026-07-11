using Dapper;
using Npgsql;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("https://0.0.0.0:7290");
var app = builder.Build();


// The secure endpoint the branches will hit
app.MapPost("/api/sync/deliveries", async ([FromBody] SyncPayload payload, IConfiguration config) =>
{
    string connString = config.GetConnectionString("PostgresCentral");

    using (var connection = new NpgsqlConnection(connString))
    {
        string sql = @"INSERT INTO CentralDeliveryLogs (BranchName, TransactionId, Date, SKU, Qty, TotalLineCost) 
               VALUES (@BranchName, @TransactionId, @Date::timestamp, @SKU, @Qty, @TotalLineCost)";

        foreach (var d in payload.Deliveries)
        {
            await connection.ExecuteAsync(sql, new
            {
                payload.BranchName,
                d.TransactionId,
                d.Date,
                d.SKU,
                d.Qty,
                d.TotalLineCost
            });
        }
    }
    return Results.Ok(new { Status = "Success", SyncedItems = payload.Deliveries.Count });
});

app.Run();

// --- Data Models matching the incoming JSON ---
public class SyncPayload
{
    public string BranchName { get; set; }
    public List<BranchDeliveryLog> Deliveries { get; set; } = new();
}

public class BranchDeliveryLog
{
    public string TransactionId { get; set; }
    public string Date { get; set; }
    public string SKU { get; set; }
    public int Qty { get; set; }
    public double TotalLineCost { get; set; }
}