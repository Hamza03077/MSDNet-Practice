using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("CSV to API Processor started...");

        string csvPath = @"C:\Users\10706557\Postman\files\DMS File Change 314.csv";
        string failedCsvPath = @"C:\Users\10706557\OneDrive - LTIMindtree\Practice\Net Core\FromBegining\DotNetMaster\CSVToJsonApp\output\FailedMoves.csv";

        string[] lines = File.ReadAllLines(csvPath);
        string[] headers = lines[0].Split(',');

        var failedRows = new List<string> { string.Join(",", headers) }; // Include header in failed CSV

        using HttpClient client = new HttpClient();
        string baseUrl = "https://ws2.apps.carrier.com/file-services-v1/move";

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            var data = new Dictionary<string, string>();

            for (int j = 0; j < headers.Length; j++)
            {
                data[headers[j].Trim()] = values[j].Trim();
            }

            if (data.ContainsKey("From") && data.ContainsKey("To"))
            {
                string fromRaw = data["From"].Trim().EndsWith(".gz") ? data["From"].Trim() : data["From"].Trim() + ".gz";
                string toRaw = data["To"].Trim().EndsWith(".gz") ? data["To"].Trim() : data["To"].Trim() + ".gz";

                string from = Uri.EscapeDataString(fromRaw);
                string to = Uri.EscapeDataString(toRaw);

                string url = $"{baseUrl}?from={from}&to={to}";

                try
                {
                    var content = new StringContent("", Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"✅ Success: Row {i} moved from '{from}' to '{to}' \n Url : " + url);
                        failedRows.Add(lines[i]);
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"❌ Failed: Row {i} - Status {response.StatusCode} \n Url : " + url);
                        failedRows.Add(lines[i]);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Error: Row {i} - {ex.Message}");
                    failedRows.Add(lines[i]);
                }
            }
            else
            {
                Console.WriteLine($"⚠️ Skipped: Row {i} missing 'From' or 'To'");
                failedRows.Add(lines[i]);
            }
        }

        // Write failed rows to a new CSV
        if (failedRows.Count > 1)
        {
            File.WriteAllLines(failedCsvPath, failedRows);
            Console.WriteLine($"📄 Failed rows saved to: {failedCsvPath}");
        }
        else
        {
            Console.WriteLine("🎉 All rows processed successfully!");
        }
    }
}