using Newtonsoft.Json.Linq;
using TronNet;
using TronNet.Crypto;
using TronLib.Tools;

namespace TronLib.Crypto
{
    internal static class CryptoOps
    {
        public static void GenerateAddress(out string privateKey, out string address)
        {
            var key = TronECKey.GenerateKey(TronNetwork.MainNet);

            privateKey = key.GetPrivateKey();
            address = key.GetPublicAddress();
        }

        public static JObject SignTransaction(JObject transaction, byte[] privateKey)
        {
            if(transaction is null || privateKey is null || privateKey.Count() == 0)
                throw new ArgumentNullException("Transaction data is empty or privateKey is wrong");
            
            var ecKey = new ECKey(privateKey, true);

            try
            {
                var hex = transaction["raw_data_hex"].ToString();

                var hexByte = hex.FromHexToByteArray();

                var hash = hexByte.ToSHA256Hash();
                var sign = ecKey.Sign(hash).ToByteArray();

                transaction.Add("signature", new JArray() { sign.ToHexString() });
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
            }

            return transaction;
        }
    }
}
