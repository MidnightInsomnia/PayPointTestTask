using System.Text;

namespace TronLib.Tools
{
    internal static class Tools
    {
        private static long _sun_unit = 1_000_000L;
        private static long hexadecimalLength = 64;
        public static decimal GetBalanceFromHex(string hex)
        {
            var parsedValue = FromHexStrToLong(hex);
            return SunToTRX(parsedValue);
        }

        private static long FromHexStrToLong(string str)
        {
            try
            {
                return Convert.ToInt64(str, 16);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPRION {ex.Message}");
            }

            return 0;
        }

        public static string ToHexString(this byte[] bytes)
        {
            var hex = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        public static byte[] FromHexToByteArray(this string input)
        {
            var numberChars = input.Length;
            var bytes = new byte[numberChars / 2];
            for (var i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static long TRXToSun(decimal trx)
        {
            try
            {
                return Convert.ToInt64(trx * _sun_unit);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPRION {ex.Message}");
            }

            return 0;
        }

        public static decimal SunToTRX(long sun)
        {
            try
            {
                return Convert.ToDecimal(sun) / _sun_unit;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPRION {ex.Message}");
            }

            return 0;
        }

        //Небольшой костыль, привожу строку с адресом к виду
        //0000000000000000000000418847c22718c97e4cf7264e462c3d88dd353fe507
        public static string NormalizeAddressHexStr(string hexStr)
        {
            var shortHexStr = hexStr.Substring(0, 42);

            var diff = hexadecimalLength - shortHexStr.Length;
            var zeroStr = "";

            for (int i = 0; i < diff; i++)
            {
                zeroStr += "0";
            }

            return $"{zeroStr}{shortHexStr}";
        }
    }
}
