
using System;
using System.Security.Cryptography;
using System.Text;

namespace BLL
{
    public class Utilities{
        public static int ToInt(string value)
        {
            int return_ = 0;

            int.TryParse(value, out return_);

            return return_;
        }

        public static decimal ToDecimal(string value)
        {
            decimal return_ = 0;

            decimal.TryParse(value, out return_);

            return return_;
        }

        public static double ToDouble(string value)
        {
            double return_ = 0;

            double.TryParse(value, out return_);

            return return_;
        }

        public static float ToFloat(string value)
        {
            float return_ = 0;

            float.TryParse(value, out return_);

            return return_;
        }

        public static string getHashSha256(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }
    }
}