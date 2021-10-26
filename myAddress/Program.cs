using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace myAddress
{
    public class Program
    {
        static void Main(string[] args)
        {
            // string data = "/+1-541-754-3010 156 Alphand_St. <J Steeve>\n 133, Green, Rd. <E Kustur> NY-56423 ;+1-541-914-3010!\n";
            string data = "156 Alphand_St. /+1-541-754-3010 <J Steeve>\n";
            string result = phone(data, "1-541-754-3010");

            Console.WriteLine(result);
        }

        public static string phone(string data, string phone)
        {
            string dataPhone = null, dataName = null, dataAddress = null;
            string phoneTrimmed = phone.Trim();
            string phoneAppend = $"+{phoneTrimmed}";

            if (phoneCount(data, phoneAppend) > 1)
            {
                return $"Error => Too many people: {phoneTrimmed}";
            }

            if (phoneCount(data, phoneAppend) == 0)
            {
                return $"Error => Not found: {phoneTrimmed}";
            }

            string[] exploded = data.Trim('\n').Split("\n");
            string findByPhone = exploded.FirstOrDefault(x => phoneCount(x, phoneAppend) == 1);
            if (!findByPhone.Equals(null))
            {
                dataPhone = phoneTrimmed;
                dataName = extractName(findByPhone);
                dataAddress = extractAddress(findByPhone, phoneAppend, $"<{dataName}>");
            }

            return $"Phone => {dataPhone}, Name => {dataName}, Address => {dataAddress}";
        }

        static int phoneCount(string data, string phone)
        {
            int count = 0;
            string phoneTrimmed = phone.Trim();

            MatchCollection matches = Regex.Matches(data, @"\+\d{1,2}-\d{3}-\d{3}-\d{4}");
            foreach (Match item in matches)
            {
                if (item.Value == phone)
                {
                    count++;
                }
            }

            return count;
        }

        static string extractName(string data)
        {
            Match match = Regex.Match(data, @"\<(.*?)\>");

            return !match.Equals(null) ? match.Groups[1].Value : null;
        }

        static string extractAddress(string data, string phone, string name)
        {
            string removedString = data.Replace(name, "").Trim().Replace(phone, "").Trim();

            Regex regex1 = new Regex(@"[^a-zA-Z0-9 - .]");
            string removeNonChar = regex1.Replace(removedString, " ");

            Regex regex2 = new Regex(@"\s+");
            string removeSpace = regex2.Replace(removeNonChar, " ");

            return removeSpace.Trim();
        }
    }
}
