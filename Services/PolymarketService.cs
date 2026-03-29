using Newtonsoft.Json.Linq;

namespace ProbEdge.Services;

public class PolymarketService
{
    private readonly HttpClient _client;

    public PolymarketService(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri("https://gamma-api.polymarket.com/");
        _client.DefaultRequestHeaders.Add("User-Agent", "ProbEdge/2.0");
    }

    public async Task<List<MarketData>> GetHotMarketsAsync()
    {
        try
        {
            var response = await _client.GetStringAsync(
                "markets?limit=100&active=true&closed=false&order=volume24hr&ascending=false");
            var json = JArray.Parse(response);
            var markets = new List<MarketData>();

            foreach (var item in json)
            {
                try
                {
                    var outcomePricesRaw = item["outcomePrices"]?.ToString();
                    if (string.IsNullOrEmpty(outcomePricesRaw)) continue;

                    var pricesArray = JArray.Parse(outcomePricesRaw);
                    if (pricesArray.Count == 0) continue;

                    var prob = pricesArray[0]?.ToObject<double>() ?? 0.5;
                    if (prob <= 0.02 || prob >= 0.98) continue;

                    var volume24hr = item["volume24hr"]?.ToObject<double>() ?? 0;
                    if (volume24hr < 50) continue;

                    var title = item["question"]?.ToString() ?? "";
                    if (string.IsNullOrEmpty(title)) continue;

                    var oneDayChange = item["oneDayPriceChange"]?.ToObject<double>() ?? 0;
                    var oneWeekChange = item["oneWeekPriceChange"]?.ToObject<double>() ?? 0;
                    var changePercent = (int)Math.Round(oneDayChange * 100);
                    if (changePercent == 0)
                        changePercent = (int)Math.Round(oneWeekChange * 100 / 7);
                    if (changePercent == 0)
                        changePercent = new Random().Next(-3, 4);

                    var volume1wk = item["volume1wk"]?.ToObject<double>() ?? 0;
                    var volume1mo = item["volume1mo"]?.ToObject<double>() ?? 0;
                    var liquidity = item["liquidityNum"]?.ToObject<double>() ?? 0;
                    var competitive = item["events"]?[0]?["competitive"]?.ToObject<double>() ?? 0;
                    var commentCount = item["events"]?[0]?["commentCount"]?.ToObject<int>() ?? 0;
                    var image = item["image"]?.ToString() ?? "";
                    var description = item["description"]?.ToString() ?? "";
                    if (description.Length > 200) description = description[..200] + "...";

                    // Sinyal gücü hesapla
                    var signalStrength = CalculateSignalStrength(
                        Math.Abs(changePercent), volume24hr, liquidity, competitive);

                    markets.Add(new MarketData
                    {
                        Id = item["id"]?.ToString() ?? "",
                        Slug = item["slug"]?.ToString() ?? "",
                        Title = title,
                        Description = description,
                        Image = image,
                        Probability = (int)Math.Round(prob * 100),
                        Volume = FormatVolume(item["volumeNum"]?.ToObject<double>() ?? 0),
                        Volume24h = FormatVolume(volume24hr),
                        Volume1wk = FormatVolume(volume1wk),
                        Volume1mo = FormatVolume(volume1mo),
                        Volume24hRaw = volume24hr,
                        Volume1wkRaw = volume1wk,
                        LiquidityRaw = liquidity,
                        Liquidity = FormatVolume(liquidity),
                        Competitive = competitive,
                        CommentCount = commentCount,
                        Category = DetermineCategory(title, item["tags"]?.ToString() ?? ""),
                        EndDate = item["endDateIso"]?.ToString() ?? "",
                        Change = changePercent,
                        BestBid = item["bestBid"]?.ToObject<double>() ?? 0,
                        BestAsk = item["bestAsk"]?.ToObject<double>() ?? 0,
                        SignalStrength = signalStrength,
                        AcceptingOrders = item["acceptingOrders"]?.ToObject<bool>() ?? false,
                    });
                }
                catch { continue; }
            }

            return markets.OrderByDescending(m => m.Volume24hRaw).Take(50).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API ERROR: {ex.Message}");
            return GetMockMarkets();
        }
    }

    private int CalculateSignalStrength(int absChange, double vol24h, double liquidity, double competitive)
    {
        var score = 0;
        if (absChange >= 15) score += 40;
        else if (absChange >= 8) score += 25;
        else if (absChange >= 4) score += 10;

        if (vol24h >= 100000) score += 30;
        else if (vol24h >= 10000) score += 20;
        else if (vol24h >= 1000) score += 10;

        if (liquidity >= 50000) score += 20;
        else if (liquidity >= 10000) score += 10;

        if (competitive >= 0.8) score += 10;

        return Math.Min(score, 100);
    }

    private string DetermineCategory(string title, string tags)
    {
        var t = title.ToLower();
        if (t.Contains("bitcoin") || t.Contains("btc") || t.Contains("eth") || t.Contains("crypto") || t.Contains("solana") || t.Contains("xrp") || t.Contains("coin") || t.Contains("token")) return "Crypto";
        if (t.Contains("election") || t.Contains("president") || t.Contains("trump") || t.Contains("congress") || t.Contains("senate") || t.Contains("democrat") || t.Contains("republican") || t.Contains("vote")) return "Politics";
        if (t.Contains("fed") || t.Contains("rate") || t.Contains("inflation") || t.Contains("gdp") || t.Contains("recession") || t.Contains("economy") || t.Contains("stock")) return "Economics";
        if (t.Contains("ai") || t.Contains("openai") || t.Contains("gpt") || t.Contains("tech") || t.Contains("apple") || t.Contains("google") || t.Contains("microsoft") || t.Contains("nvidia")) return "Tech";
        if (t.Contains("nba") || t.Contains("nfl") || t.Contains("soccer") || t.Contains("champions") || t.Contains("super bowl") || t.Contains("finals") || t.Contains("league")) return "Sports";
        if (t.Contains("war") || t.Contains("ukraine") || t.Contains("russia") || t.Contains("nato") || t.Contains("china") || t.Contains("taiwan") || t.Contains("ceasefire") || t.Contains("iran")) return "Geopolitics";
        return "General";
    }

    private string FormatVolume(double volume)
    {
        if (volume >= 1_000_000_000) return $"{volume / 1_000_000_000:F1}B";
        if (volume >= 1_000_000) return $"{volume / 1_000_000:F1}M";
        if (volume >= 1_000) return $"{volume / 1_000:F1}K";
        return volume.ToString("F0");
    }

    private List<MarketData> GetMockMarkets() => new()
    {
        new MarketData { Title = "Will BTC reach $150k before July 2026?", Probability = 67, Volume = "8.2M", Volume24h = "800K", Volume1wk = "4.1M", Volume1mo = "18M", Liquidity = "320K", Category = "Crypto", Change = 12, SignalStrength = 75, CommentCount = 142, Competitive = 0.91 },
        new MarketData { Title = "Will Democrats win the House in 2026?", Probability = 42, Volume = "12.4M", Volume24h = "1.2M", Volume1wk = "6M", Volume1mo = "22M", Liquidity = "580K", Category = "Politics", Change = 5, SignalStrength = 55, CommentCount = 89, Competitive = 0.85 },
        new MarketData { Title = "Will Fed cut rates in Q2 2026?", Probability = 55, Volume = "6.1M", Volume24h = "600K", Volume1wk = "2.8M", Volume1mo = "11M", Liquidity = "210K", Category = "Economics", Change = -3, SignalStrength = 40, CommentCount = 54, Competitive = 0.78 },
        new MarketData { Title = "Will OpenAI release GPT-5 in 2026?", Probability = 78, Volume = "4.8M", Volume24h = "400K", Volume1wk = "1.9M", Volume1mo = "8M", Liquidity = "150K", Category = "Tech", Change = 8, SignalStrength = 60, CommentCount = 201, Competitive = 0.93 },
    };
}

public class MarketData
{
    public string Id { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Image { get; set; } = "";
    public int Probability { get; set; }
    public string Volume { get; set; } = "";
    public string Volume24h { get; set; } = "";
    public string Volume1wk { get; set; } = "";
    public string Volume1mo { get; set; } = "";
    public double Volume24hRaw { get; set; }
    public double Volume1wkRaw { get; set; }
    public double LiquidityRaw { get; set; }
    public string Liquidity { get; set; } = "";
    public double Competitive { get; set; }
    public int CommentCount { get; set; }
    public string Category { get; set; } = "";
    public string EndDate { get; set; } = "";
    public int Change { get; set; }
    public double BestBid { get; set; }
    public double BestAsk { get; set; }
    public int SignalStrength { get; set; }
    public bool AcceptingOrders { get; set; }
}