using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TronLib.Api
{
    internal class TronApiRequest
    {
        private static HttpClient httpClient = new HttpClient();

        private readonly string TRONUrl = "https://api.nileex.io/";

        private readonly string TRONGridUrl = "https://nile.trongrid.io/v1/";

        public TronApiRequest()
        {
            httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public string GetAccount(string address)
        {
            string URL = TRONUrl + "wallet/getaccount";

            var JSON = new JObject()
            {
                { "address", address },
                { "visible" , true }
            };

            return SendRequest(URL, JSON);
        }

        public string ValidateAddress(string address)
        {
            string URL = TRONUrl + "wallet/validateaddress";

            var JSON = new JObject()
            {
                { "address", address },
                { "visible" , true }
            };

            return SendRequest(URL, JSON);
        }

        public string CreateTransaction(string from, string to, long amount)
        {
            string URL = TRONUrl + "wallet/createtransaction";

            var JSON = new JObject()
                {
                    { "owner_address", from },
                    { "to_address", to },
                    { "amount" , amount },
                    { "visible" , true }
                };

            return SendRequest(URL, JSON);
        }

        public string BroadcastTransaction(JObject signedTransaction)
        {
            string URL = TRONUrl + "wallet/broadcasttransaction";

            return SendRequest(URL, signedTransaction);
        }

        public string GetBalance(string address, string addressHex, string contractAddress)
        {
            string URL = TRONUrl + "wallet/triggersmartcontract";

            var JSON = new JObject()
            {
                { "contract_address", contractAddress },
                { "parameter", addressHex },
                { "function_selector", "balanceOf(address)" },
                { "owner_address", address },
                { "visible" , true }
            };

            return SendRequest(URL, JSON);
        }

        public async Task<string> GetTransactionHistory(string accountBase58, string token, DateTime minTimeStamp, DateTime maxTimeStamp)
        {
            try
            {
                var minTime = ((DateTimeOffset)minTimeStamp).ToUnixTimeMilliseconds();
                var maxTime = ((DateTimeOffset)maxTimeStamp).ToUnixTimeMilliseconds();

                var URL = $"{TRONGridUrl}accounts/" +
                          $"{accountBase58}/transactions/" +
                          $"{token}" +
                          $"?min_timestamp={minTime}" +
                          $"&max_timestamp={maxTime}";

                var response = httpClient.GetAsync(URL).Result;
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
                return String.Empty;
            }
        }

        private string SendRequest(string URL, JObject JSON)
        {
            string response = Request(URL, JSON).Result;

            return response;
        }

        private async Task<string> Request(string URL, JObject JSON)
        {
            try
            {
                var content = JsonConvert.SerializeObject(JSON);
                HttpContent cont = new StringContent(content, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(URL, cont);
                var responseString = await response.Content.ReadAsStringAsync();

                //Console.WriteLine($"{URL} \n\n {responseString} \n\n ");
                return responseString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
                return String.Empty;
            }
        }
    }
}
