using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ZodiacService.Operations;

namespace ZodiacService.Services
{
    public class ZodiacService : Horoscope.HoroscopeBase
    {
        private readonly ZodiacOperations _zodiacOperations = new ZodiacOperations();

        private readonly ILogger<ZodiacService> _logger;
        public ZodiacService(ILogger<ZodiacService> logger)
        {
            _logger = logger;
        }
        public static string GetSign(Zodiac zodiac)
        {
            var zodiacs = ZodiacOperations.GetAllZodiacs();

            return (from variable in zodiacs

                    let startMonth = int.Parse(variable.Item1.Date.Substring(0, 2))
                    let startDay = int.Parse(variable.Item1.Date.Substring(3, 2))
                    let endMonth = int.Parse(variable.Item2.Date.Substring(0, 2))
                    let endDay = int.Parse(variable.Item2.Date.Substring(3, 2))

                    let thisMonth = int.Parse(zodiac.Date.Substring(0, 2))
                    let thisDay = int.Parse(zodiac.Date.Substring(3, 2))

                    where thisMonth == startMonth && thisDay >= startDay || thisMonth == endMonth && thisDay <= endDay
                    select variable.Item3).FirstOrDefault();
        }
        public override Task<AddZodiacResponse> AddZodiac(AddZodiacRequest request, ServerCallContext context)
        {
            var zodiac = request.Zodiac;
            string sign = default;
            var thisYear = int.Parse(zodiac.Date.Substring(6, 4));
            var thisMonth = int.Parse(zodiac.Date.Substring(0, 2));
            var thisDay = int.Parse(zodiac.Date.Substring(3, 2));

            var dateTime = new DateTime(thisYear, thisMonth, thisDay);


            sign = GetSign(zodiac);
            Console.WriteLine($"\nDate: {zodiac.Date}");
            Console.WriteLine($"Sign: {sign.ToString()}\n");
            return Task.FromResult(new AddZodiacResponse()
            {
                Status = AddZodiacResponse.Types.Status.Success,
                Sign = sign
            });
        }
    }
}