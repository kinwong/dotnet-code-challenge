using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BetEasy
{
    static class FeedReader
    {
        public static IEnumerable<HorseDetail> Read(string pathName)
        {
            var attr = File.GetAttributes(pathName);
            var horses = ((attr & FileAttributes.Directory) == FileAttributes.Directory) ?
                ReadDirectory(pathName) : ReadFile(pathName);
            foreach (var horse in horses)
            {
                yield return horse;
            }
        }
        private static IEnumerable<HorseDetail> ReadDirectory(string directoryName)
        {
            return from fileName in Directory.EnumerateFiles(directoryName)
                   from horse in FeedReader.Read(fileName)
                   select horse;
        }
        private static IEnumerable<HorseDetail> ReadFile(string fileName)
        {
            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new StreamReader(stream))
            {
                var extension = Path.GetExtension(fileName);
                switch (extension.ToLowerInvariant())
                {
                    case ".json":
                        foreach (var horse in JsonFeedParser.Parse(reader))
                        {
                            yield return horse;
                        }
                        break;

                    case ".xml":
                        foreach (var horse in XmlFeedParser.Parse(reader))
                        {
                            yield return horse;
                        }
                        break;

                    default:
                        // No horse is returned for Unhandled file type.
                        yield break;
                }
            }
        }
    }
}
