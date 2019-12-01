using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

[assembly: InternalsVisibleTo("dotnet_code_challenge.Test")]
namespace BetEasy
{
    static class XmlFeedParser
    {
        public static IEnumerable<HorseDetail> Parse(TextReader reader)
        {
            var xDoc = XDocument.Load(reader);
            var meeting = xDoc.Root;
            if (meeting.Name != "meeting")
                throw new Exception("Root element 'meeting' is expected.");
                
            var races = meeting.Element("races").Elements("race");
            foreach (var race in races)
            {
                var prices = 
                    from price in race.Element("prices").Elements("price")
                    from horse in price.Element("horses").Elements("horse")
                    select new
                    {
                        HorseNumber = horse.Attribute("number").Value,
                        Value = (decimal)horse.Attribute("Price")
                    };
                var priceMap = prices.ToDictionary(
                    price => price.HorseNumber, price => price.Value);

                var horses = race.Elements("horses").Elements("horse");
                foreach (var horse in horses)
                {
                    var horseNumber = (string)horse.Element("number");
                    yield return new HorseDetail
                    {
                        Race = race.Attribute("name").Value,
                        Name = horse.Attribute("name").Value,
                        Price = priceMap.GetValueOrDefault(horseNumber),
                    };
                }
            }
        }
   }
}
