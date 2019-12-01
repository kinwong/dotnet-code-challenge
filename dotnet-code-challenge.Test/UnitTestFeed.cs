using Xunit;
using BetEasy;
using System.IO;
using System.Linq;

namespace dotnet_code_challenge.Test
{
    public class UnitTestFeed
    {
        [Fact]
        public void TestXmlParser()
        {
            var horses = XmlFeedParser.Parse(File.OpenText("sample.xml"))
                .OrderBy(horse => horse.Name)
                .ToArray();

            Assert.Equal(2, horses.Length);
            Assert.Equal("Advancing", horses[0].Name);
            Assert.Equal(4.2M, horses[0].Price);
            Assert.Equal("Coronel", horses[1].Name);
            Assert.Equal(12M, horses[1].Price);
        }
        [Fact]
        public void TestJsonParser()
        {
            var horses = JsonFeedParser.Parse(File.OpenText("sample.json"))
                .OrderBy(horse => horse.Name)
                .ToArray();

            Assert.Equal(2, horses.Length);
            Assert.Equal("Fikhaar", horses[0].Name);
            Assert.Equal(4.4M, horses[0].Price);
            Assert.Equal("Toolatetodelegate", horses[1].Name);
            Assert.Equal(10M, horses[1].Price);
        }
    }
}
