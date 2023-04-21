using System.Net;
using Newtonsoft.Json;

public class Huobi
{
    private const string API_BASE = "https://api.huobi.pro";
    public static MarketHistoryKlineResponse[] MarketHistoryKLine(string symbol, string period, int size)
    {
        using (var client = new WebClient())
        {
            var url = $"{API_BASE}/market/history/kline?" + $"symbol={symbol}&period={period}&size={size}";
            var response = FromJson<Response<MarketHistoryKlineResponse[]>>(client.DownloadString(url));
            if (string.CompareOrdinal("ok", response.status) == 0)
                return response.data;
            else
                return null;
        }
    }

    public static void Main()
    {
        var json = Huobi.MarketHistoryKLine("btcusdt", "1day", 20);
        Console.WriteLine(json);
    }


    public static T FromJson<T>(string json)
    {

        if (!string.IsNullOrEmpty(json))
        {
            var item = JsonConvert.DeserializeObject<T>(json);
            return item;
        }
        else
        {
            return default(T);
        }
    }

    private class Response<T>
    {
        public string? status { get; set; }

        public string? ch { get; set; }

        public long ts { get; set; }

        public T? data { get; set; }
    }
}

public class MarketHistoryKlineResponse
{
    public long id { get; set; }
    public decimal amount { get; set; }

    public int count { get; set; }


    public decimal open { get; set; }

    public decimal close { get; set; }

    public decimal low { get; set; }


    public decimal hight { get; set; }

    public decimal vol { get; set; }
}
