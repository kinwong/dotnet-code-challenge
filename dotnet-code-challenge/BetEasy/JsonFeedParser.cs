using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BetEasy
{
    static class JsonFeedParser
    {
        public static IEnumerable<HorseDetail> Parse(TextReader reader)
        {
            using(var jsonReader = new JsonTextReader(reader))
            {
                var rawData = (JObject)JObject.Load(jsonReader)["RawData"];

                var prices = 
                    from market in ((JArray)rawData["Markets"]).Cast<JObject>()
                    from selection in ((JArray)market["Selections"]).Cast<JObject>()
                    select new 
                    {
                        HorseNumber = (string)selection.Property("Id"),
                        Price = (decimal)selection.Property("Price"),
                        Name = (string)((JObject)selection["Tags"]).Property("name")
                    };
                foreach (var price in prices)
                {
                    yield return new HorseDetail
                    {
                        Name = price.Name,
                        Price = price.Price,
                        Race = (string)rawData.Property("FitureName")
                    };
                }
            }
        }
    }
}