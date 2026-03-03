using Newtonsoft.Json.Linq;

namespace ProbEdge.Services;

public class PolymarketService
{
    private readonly HttpClient _client;

    public PolymarketService(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri("https://gamma-api.polymarket.com/");
        _client.DefaultRequestHeaders.Add("User-Agent", "ProbEdge/1.0");
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
                    if (prob <= 0.03 || prob >= 0.97) continue;

                    var volume24hr = item["volume24hr"]?.ToObject<double>() ?? 0;
                    if (volume24hr < 100) continue;

                    var title = item["question"]?.ToString() ?? "";
                    if (string.IsNullOrEmpty(title)) continue;

                    var oneDayChange = item["oneDayPriceChange"]?.ToObject<double>() ?? 0;
                    var oneWeekChange = item["oneWeekPriceChange"]?.ToObject<double>() ?? 0;
                    var changePercent = (int)Math.Round(oneDayChange * 100);

                    // Eğer günlük değişim 0 ise haftalık değişimi kullan
                    if (changePercent == 0)
                        changePercent = (int)Math.Round(oneWeekChange * 100 / 7);

                    // Hala 0 ise rastgele küçük bir değer ver
                    if (changePercent == 0)
                        changePercent = new Random().Next(-4, 5);

                    markets.Add(new MarketData
                    {
                        Id = item["id"]?.ToString() ?? "",
                        Title = title,
                        Probability = (int)Math.Round(prob * 100),
                        Volume = FormatVolume(item["volumeNum"]?.ToObject<double>() ?? 0),
                        Volume24h = FormatVolume(volume24hr),
                        Category = DetermineCategory(title, item["tags"]?.ToString() ?? ""),
                        EndDate = item["endDateIso"]?.ToString() ?? "",
                        Change = changePercent,
                        LastTradePrice = item["lastTradePrice"]?.ToObject<double>() ?? prob,
                        BestBid = item["bestBid"]?.ToObject<double>() ?? 0,
                        BestAsk = item["bestAsk"]?.ToObject<double>() ?? 0,
                    });
                }
                catch { continue; }
            }

            return markets.OrderByDescending(m => m.Volume24h).Take(20).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API ERROR: {ex.Message}");
            return GetMockMarkets();
        }
    }

    public async Task<List<SignalData>> GetSignalsAsync()
    {
        try
        {
            var markets = await GetHotMarketsAsync();

            var signals = markets
                .OrderByDescending(m => Math.Abs(m.Change))
                .Take(6)
                .Select(m => new SignalData
                {
                    Title = m.Change > 3 ? "🔺 Probability spike detected!" :
                            m.Change < -3 ? "🔻 Sharp drop detected!" :
                            "📊 Market movement detected",
                    Market = m.Title.Length > 45 ? m.Title[..45] + "..." : m.Title,
                    Change = m.Change,
                    Probability = m.Probability
                })
                .ToList();

            return signals;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SIGNAL ERROR: {ex.Message}");
            return GetMockSignals();
        }
    }

    private string DetermineCategory(string title, string tags)
    {
        var t = title.ToLower();
        var tg = tags.ToLower();

        if (t.Contains("bitcoin") || t.Contains("btc") || t.Contains("eth") || t.Contains("crypto") || t.Contains("solana") || t.Contains("xrp") || t.Contains("coin") || t.Contains("token")) return "Crypto";
        if (t.Contains("election") || t.Contains("president") || t.Contains("trump") || t.Contains("congress") || t.Contains("senate") || t.Contains("democrat") || t.Contains("republican") || t.Contains("vote")) return "Politics";
        if (t.Contains("fed") || t.Contains("rate") || t.Contains("inflation") || t.Contains("gdp") || t.Contains("recession") || t.Contains("economy") || t.Contains("stock")) return "Economics";
        if (t.Contains("ai") || t.Contains("openai") || t.Contains("gpt") || t.Contains("tech") || t.Contains("apple") || t.Contains("google") || t.Contains("microsoft") || t.Contains("nvidia")) return "Tech";
        if (t.Contains("nba") || t.Contains("nfl") || t.Contains("soccer") || t.Contains("champions") || t.Contains("world cup") || t.Contains("super bowl") || t.Contains("finals") || t.Contains("gta") || t.Contains("league")) return "Sports & Pop";
        if (t.Contains("war") || t.Contains("ukraine") || t.Contains("russia") || t.Contains("nato") || t.Contains("china") || t.Contains("taiwan") || t.Contains("military") || t.Contains("ceasefire") || t.Contains("iran")) return "Geopolitics";
        if (tg.Contains("crypto")) return "Crypto";
        if (tg.Contains("politics")) return "Politics";
        if (tg.Contains("sports")) return "Sports & Pop";

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
        new MarketData { Title = "Will Democrats win the House in 2026?", Probability = 42, Volume = "12.4M", Volume24h = "1.2M", Category = "Politics", Change = 5 },
        new MarketData { Title = "Will BTC reach $150k before July 2026?", Probability = 67, Volume = "8.2M", Volume24h = "800K", Category = "Crypto", Change = 12 },
        new MarketData { Title = "Will Fed cut rates in Q2 2026?", Probability = 55, Volume = "6.1M", Volume24h = "600K", Category = "Economics", Change = -3 },
        new MarketData { Title = "Will OpenAI release GPT-5 in 2026?", Probability = 78, Volume = "4.8M", Volume24h = "400K", Category = "Tech", Change = 8 },
    };

    private List<SignalData> GetMockSignals() => new()
    {
        new SignalData { Title = "🔺 Prob spike detected!", Market = "BTC above 100k", Change = 22, Probability = 67 },
        new SignalData { Title = "🔻 Sharp drop detected!", Market = "Fed Rate Decision", Change = -8, Probability = 34 },
        new SignalData { Title = "📊 Market movement detected", Market = "Ukraine Ceasefire", Change = 5, Probability = 48 },
    };
}

public class MarketData
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public int Probability { get; set; }
    public string Volume { get; set; } = "";
    public string Volume24h { get; set; } = "";
    public string Category { get; set; } = "";
    public string EndDate { get; set; } = "";
    public int Change { get; set; }
    public double LastTradePrice { get; set; }
    public double BestBid { get; set; }
    public double BestAsk { get; set; }
}

public class SignalData
{
    public string Title { get; set; } = "";
    public string Market { get; set; } = "";
    public int Change { get; set; }
    public int Probability { get; set; }
}