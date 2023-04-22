using System.Net;
using System.Net.WebSockets;
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


public class HuobiWs
{
    private static WebSocket websocket;
    private static Dictionary<string, string> topicDic =
        new Dictionary<string, string>();
    private static bool isOpened = false;
    private const string HUOBI_WEBSOCKET_API = "wss://api.huobi.pro/ws";
    #region  市场信息常量
    public const string MARKET_KLINE = "market.{0}.kline.{1}";
    public const string MARKET_DEPTH = "market.{0}.depth.{1}";
    public const string MARKET_TRADE_DETAIL = "market.{0}.trade.detail";
    public const string MARKET_DETAIL = "market.{0}.detail";
    #endregion
    public static event EventHandler<MessageReceivedEventArgs> OnMessage;
    public static bool Init()
    {
        websocket = new WebSocket(HUOBI_WEBSOCKET_API);
        websocket.Security.EnabledSslProtocols = SslProtocols.Tls12;
        websocket.Opened += OnOpened;
         websocket.DataReceived += ReceivedMsg;
         websocket.Error += OnError;
         websocket.Open();
         return true;
     }

     private static void OnError(object sender, EventArgs e)
     {
         Console.WriteLine("Error:" + e.ToString());
     }

     public static void OnOpened(object sender, EventArgs e)
     {
         Console.WriteLine($"OnOpened Topics Count:{topicDic.Count}");
         isOpened = true;
         foreach (var item in topicDic)
             SendSubscribeTopic(item.Value);
     }

     public static void ReceivedMsg(object sender, DataReceivedEventArgs args)
     {
         var msg = GZipHelper.GZipDecompressString(args.Data);
         if (msg.IndexOf("ping") != -1) //响应心跳包
         {
             var reponseData = msg.Replace("ping", "pong");
             websocket.Send(reponseData);
         }
         else//接收消息
         {
             OnMessage?.Invoke(
                 null, new MessageReceivedEventArgs() { Message = msg });
         }
     }

     public static string toJson(object obj) {
        return JsonConvert.SerializeObject(obj);
     }

     public static void Subscribe(string topic, string id)
     {
         if (topicDic.ContainsKey(topic))
             return;
         var msg = toJson(new {sub = topic, id });
         topicDic.Add(topic, msg);
         if (isOpened)
             SendSubscribeTopic(msg);
     }

     public static void UnSubscribe(string topic, string id)
     {
         if (!topicDic.ContainsKey(topic) || !isOpened)
             return;
         var msg = $"{{\"unsub\":\"{topic}\",\"id\":\"{id}\"}}";
         topicDic.Remove(topic);
        SendSubscribeTopic(msg);
         Console.WriteLine($"UnSubscribed {topic}");
     }
     private static void SendSubscribeTopic(string msg)
     {
         websocket.Send(msg);
        Console.WriteLine(msg);
     }

     public class MessageReceivedEventArgs : EventArgs
    {
         public string Message { get; set; }
    }
}