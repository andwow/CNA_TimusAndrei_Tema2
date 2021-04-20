using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using ZodiacClient;

namespace ZodiacClient
{
    class Program
    {
        public static bool ValidateDate(string date)
        {

            var month = int.Parse(date.Substring(0, 2));
            var day = int.Parse(date.Substring(3, 2));
            var year = int.Parse(date.Substring(6, 4));

            try
            {
                var dateTime = new DateTime(year, month, day);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Invalid Date!");
                return false;
            }

            var regex = new Regex("([0-9]{2})/([0-9]{2})/([0-9]{4})");
            var match = regex.Match(date);

            return match.Success;
        }
    }
}
