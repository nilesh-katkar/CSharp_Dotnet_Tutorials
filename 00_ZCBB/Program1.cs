

using Newtonsoft.Json;

class Program1
{
    public static void Main1()
    {
        Console.WriteLine("Hello!");

        string url = "http://10.129.137.72:8080/obankApi/bankPlus";
        SendSecureRequest(url);
    }

    public static string SendSecureRequest(string url)
    {
        try
        {
            // 1. Construct RequestService
            //var requestService = new HttpSendJSONService(url, 15000, 15000, "POST");

            // 2. Get the client (Singleton pattern, just like Java)
            //var client = ClientFactory.GetClient(requestService);

            // 3. Prepare Payload Maps
            var publicMap = new Dictionary<string, object>
        {
            { "ip", "223.104.247.109" },
            { "osType", Environment.OSVersion.ToString() },
            { "uuid", Guid.NewGuid().ToString() },
            { "imei", Guid.NewGuid().ToString() },
            { "phoneType", "HUAWEI META 10" },
            { "mac", "005056BD1488" },
            { "address", "浙江,杭州,上城区" }
        };

            var subMap = new Dictionary<string, object>
        {
            { "publicData", publicMap },
            { "openId", "5650401403990002" },
            { "acctNo", "NRA56504142010500000437" }
        };

            var topMap = new Dictionary<string, object>
        {
            { "requestId", "2022" + DateTimeOffset.Now.ToUnixTimeMilliseconds() },
            { "channelId", "Tangopay" },
            { "interfaceId", "od0001" },
            { "timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() },
            { "signature", "signature" },
            { "bizData", subMap }
        };

            // 4. Convert to JSON
            string jsonString = JsonConvert.SerializeObject(topMap);
            Console.WriteLine("----- Original Payload -----");
            Console.WriteLine(jsonString);

            // 5. Send via client
            //string response = client.Send(jsonString);
            Console.WriteLine("----- Final Response -----");
            //Console.WriteLine(response);

            //return response;
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in secure request: " + ex.Message);
            return null;
        }
    }


    public static void RandomKeyGenerate()
    {

    }

    public static void EncryptBizData()
    {

    }

    public static void GetSignature()
    {

    }
}
