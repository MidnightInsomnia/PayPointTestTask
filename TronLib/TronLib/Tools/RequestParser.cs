using Newtonsoft.Json.Linq;

namespace TronLib.Tools
{
    internal static class RequestParser
    {
        internal static JObject ParseResponse(string response)
        {
            try
            {
                return JObject.Parse(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPRION {ex.Message}");
                return null;
            }
        }
        internal static bool ParseValidationStatus(string json)
        {
            try
            {
                var result = JObject.Parse(json);

                if (result["result"] != null)
                    return (bool)result["result"];

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION {ex.Message}");
                return false;
            }
        }

        internal static JObject ParseAccountInfo(string request)
        {
            try
            {
                return JObject.Parse(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
                return null;
            }
        }

        internal static decimal ParseTRXBalance(string request)
        {
            try
            {
                var res = "";

                var result = JObject.Parse(request);

                Console.WriteLine($"JSON {result}");

                try
                {
                    res = result["balance"].ToString();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"EXCEPTION {ex.Message}");
                    res = "0";
                }

                long.TryParse(res, out var sunBalanse);

                return Tools.SunToTRX(sunBalanse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
                return 0;
            }
        }

        internal static decimal ParseTRC20Balance(string request)
        {
            try
            {
                var result = JObject.Parse(request);
                var res = "";

                try
                {
                    res = result["constant_result"][0].ToString();
                }
                catch
                {
                    res = "0";
                }

                return Tools.GetBalanceFromHex(res);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
                return 0;
            }
        }

        internal static string ParseBroadcastResult(string transactionResult)
        {
            try
            {
                var transactionResJSON = JObject.Parse(transactionResult);

                if (transactionResJSON["result"].ToString().ToLower().Equals("true"))
                    return transactionResJSON["txid"].ToString();
                else
                    return "TRANSACTION BROADCAST FAILED";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPRION {ex.Message}");
                return String.Empty;
            }
        }
    }
}
