using Newtonsoft.Json.Linq;
using TronLib.Api;
using TronLib.Crypto;
using TronLib.Enums;
using TronLib.Tools;
using TronNet;

namespace TronLib
{
    /// <summary>
    /// Класс <c>Tron</c> обеспечивает моё трудоустройство (I hope so 🙂)
    /// </summary>
    public static class Tron
    {
        private static TronApiRequest tronApi = new TronApiRequest();

        //Адреса контрактов TRC20
        private const string USDTContract = "TXYZopYRdj2D9XRtbG411XZZ3kM5VkAeBf";
        private const string USDDContract = "TFT7sNiNDGZcqL7z7dwXUPpxrx1Ewk8iGL";
        private const string USDСContract = "TEMVynQpntMqkPxP6wXTW2K7e4sM3cRmWz";

        /// <summary>
        /// Метод <c>GenerateAddress</c> Генерирует приватный ключ и TRON адрес
        /// </summary>
        /// <param name="privateKey">Строка для записи результата генерации приватного ключа.</param>
        /// <param name="address">Строка для записи результата генерации адреса.</param>
        public static void GenerateAddress(out string privateKey, out string address)
        {
            CryptoOps.GenerateAddress(out privateKey, out address);
        }

        /// <summary>
        /// Метод <c>IsAddressValid</c> Проверяет является ли адрес TRON действительным
        /// </summary>
        /// <param name="address">TRON адрес для проверки</param>
        /// <returns>True - если адрес действителен, False - если нет.</returns>
        public static bool IsAddressValid(string address)
        {
            if(String.IsNullOrEmpty(address))
                throw new ArgumentNullException("EMPTY ADDRESS");

            var result = tronApi.ValidateAddress(address);

            return RequestParser.ParseValidationStatus(result);
        }

        /// <summary>
        /// Метод <c>GetAccountInfo</c> Получает информацию о TRON аккаунте
        /// </summary>
        /// <param name="address">TRON адрес</param>
        /// <returns>JSON с данными аккаунта</returns>
        public static JObject GetAccountInfo(string address)
        {
            if (String.IsNullOrEmpty(address))
                throw new ArgumentNullException("EMPTY ADDRESS");

            var response = tronApi.GetAccount(address);

            return RequestParser.ParseAccountInfo(response);
        }

        /// <summary>
        /// Метод <c>GetTRXBalance</c> Получает баланс TRX токена на аккаунте
        /// </summary>
        /// <param name="address">TRON адрес</param>
        /// <returns>decimal значение баланса</returns>
        public static decimal GetTRXBalance(string address)
        {
            if (String.IsNullOrEmpty(address))
                throw new ArgumentNullException("EMPTY ADDRESS");

            var response = tronApi.GetAccount(address);

            return RequestParser.ParseTRXBalance(response);
        }

        /// <summary>
        /// Метод <c>GetUSDTBalance</c> Получает баланс USDT токена на аккаунте
        /// </summary>
        /// <param name="address">TRON адрес</param>
        /// <returns>decimal значение баланса</returns>
        public static decimal GetUSDTBalance(string address)
        {
            if (String.IsNullOrEmpty(address))
                throw new ArgumentNullException("EMPTY ADDRESS");

            return GetAccountTRC20Balance(address, TRC20Token.USDT);
        }

        /// <summary>
        /// Метод <c>GetUSDCBalance</c> Получает баланс USDC токена на аккаунте
        /// </summary>
        /// <param name="address">TRON адрес</param>
        /// <returns>decimal значение баланса</returns>
        public static decimal GetUSDCBalance(string address)
        {
            if (String.IsNullOrEmpty(address))
                throw new ArgumentNullException("EMPTY ADDRESS");

            return GetAccountTRC20Balance(address, TRC20Token.USDC);
        }

        /// <summary>
        /// Метод <c>GetUSDDBalance</c> Получает баланс USDD токена на аккаунте
        /// </summary>
        /// <param name="address">TRON адрес</param>
        /// <returns>decimal значение баланса</returns>
        public static decimal GetUSDDBalance(string address)
        {
            if (String.IsNullOrEmpty(address))
                throw new ArgumentNullException("EMPTY ADDRESS");

            return GetAccountTRC20Balance(address, TRC20Token.USDD);
        }

        /// <summary>
        /// Метод <c>GetAccountTRC20Balance</c> Получает баланс токенов стандарта TRC20 на аккаунте
        /// </summary>
        /// <param name="address">TRON адрес</param>
        /// <param name="trc20Token">Константа с типом TRC20 токена</param>
        /// <returns>decimal значение баланса</returns>
        private static decimal GetAccountTRC20Balance(string address, TRC20Token trc20Token)
        {
            if (address.IsHex())
                throw new ArgumentException("ADDRESS SHOULD BE BASE58");

            var contractAddress = "";

            switch (trc20Token)
            {
                case TRC20Token.USDC:
                    contractAddress = USDСContract;
                    break;

                case TRC20Token.USDD:
                    contractAddress = USDDContract;
                    break;

                default:
                    contractAddress = USDTContract;
                    break;
            }

            var addressBytes = Base58.Decode(address);

            var tmpHex = Tools.Tools.ToHexString(addressBytes);
            var hexAddress = Tools.Tools.NormalizeAddressHexStr(tmpHex);

            var response = tronApi.GetBalance(address, hexAddress, contractAddress);

            return RequestParser.ParseTRC20Balance(response);
        }

        /// <summary>
        /// Метод <c>MakeTRXTransfer</c> создаёт, подписывает и транслирует транзакцию с переводом TRX токена в сеть
        /// </summary>
        /// <param name="from">TRON адрес отправителя</param>
        /// <param name="to">TRON адрес получателя</param>
        /// <param name="trxAmount">Количество TRX токенов для отправки</param>
        /// <param name="privateKey">Приватный ключ отправителя</param>
        /// <returns>Хеш транзакции</returns>
        public static string MakeTRXTransfer(string from, string to, decimal trxAmount, string privateKey)
        {
            if (String.IsNullOrEmpty(from) 
                || String.IsNullOrEmpty(to) 
                || trxAmount <= 0 
                || String.IsNullOrEmpty(privateKey))
                throw new ArgumentException("INCORREST PARAMETERS");

            var sunAmount = Tools.Tools.TRXToSun(trxAmount);
            var response = tronApi.CreateTransaction(from, to, sunAmount);

            var unsignedTransaction = RequestParser.ParseResponse(response);

            var key = Tools.Tools.FromHexToByteArray(privateKey);
            var signedTransaction = CryptoOps.SignTransaction(unsignedTransaction, key);
            var transactionResult = tronApi.BroadcastTransaction(signedTransaction);

            return RequestParser.ParseBroadcastResult(transactionResult);
        }

        /// <summary>
        /// Метод <c>GetTransactionHistory</c> получает список операций в период между указанными датами
        /// </summary>
        /// <param name="address">TRON адрес</param>
        /// <param name="minTimestamp">минимальная дата</param>
        /// <param name="maxTimestamp">максимальная дата</param>
        /// <returns>JSON со списком транзакций</returns>
        public static JObject GetTransactionHistory(string address, DateTime minTimestamp, DateTime maxTimestamp)
        {
            if (String.IsNullOrEmpty(address))
                throw new ArgumentNullException("EMPTY ADDRESS");

            if (minTimestamp > maxTimestamp)
                throw new ArgumentException("INCORRECT TIMESTAMPS");

            var trxHistory = tronApi.GetTransactionHistory(address, "", minTimestamp, maxTimestamp).Result;
            var trcHistory = tronApi.GetTransactionHistory(address, "trc20", minTimestamp, maxTimestamp).Result;

            var trxJson = RequestParser.ParseResponse(trxHistory);
            var trcJson = RequestParser.ParseResponse(trcHistory);

            var resultJson = new JObject
                {
                    { "TRX_HISTORY", trxJson },
                    { "TRC20_HISTORY", trcJson },
                };

            return resultJson;
        }
    }
}