using TronLib;

var ownerAddress = "TMxZJUHiKfTQypw6EmUSjJ86iQWXsqynhZ";
var ownerFriendAddress = "THc4Dsh6D4VRHB5Cy2GiGi52z2HAUK6xSV";
var ownerPrivateKey = "810110e63198035555ac910b211b556cc3891d1b5e1e73b14de2e5e22a09d3ac";

//PRIVATE KEY & ADDRESS GENERATION
/*Tron.GenerateAddress(out var privateKey, out var address);
Console.WriteLine($"KEY: {privateKey} \nADDRESS: {address}");*/

//ADDRESS VALIDATION
/*var valid = Tron.IsAddressValid(ownerAddress);
 * 
if (valid)
    Console.WriteLine($"ADDRESS: {ownerAddress} IS VALID");
else
    Console.WriteLine($"ADDRESS: {ownerAddress} IS NOT VALID");

//ACCOUNT INFO JSON
var InfoJSON = Tron.GetAccountInfo(ownerAddress);
if (InfoJSON != null) Console.WriteLine($"ACCOUNT JSON:\n{InfoJSON}");*/

var trxBalance = Tron.GetTRXBalance(ownerAddress);
/*var usdtBalance = Tron.GetUSDTBalance(ownerAddress);
var usdcBalance = Tron.GetUSDCBalance(ownerAddress);
var usddBalance = Tron.GetUSDDBalance(ownerAddress);

Console.WriteLine($"BALANCE: {trxBalance} TRX");
Console.WriteLine($"BALANCE: {usdtBalance} USDT");
Console.WriteLine($"BALANCE: {usdcBalance} USDC");
Console.WriteLine($"BALANCE: {usddBalance} USDD");*/


/*var transactionHash = Tron.MakeTRXTransfer(ownerAddress, ownerFriendAddress, 500, ownerPrivateKey);
if (!String.IsNullOrEmpty(transactionHash)) Console.WriteLine($"TRANSACTION HASH: {transactionHash}");*/


DateTime myDate = DateTime.Parse("2023-04-25 00:00:00");
var historyJSON = Tron.GetTransactionHistory(ownerAddress, myDate, DateTime.Now);
if (historyJSON != null) Console.WriteLine($"TRANSACTIONS HISTORY: \n{historyJSON}");