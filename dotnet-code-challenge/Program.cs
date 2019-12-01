using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BetEasy;


namespace dotnet_code_challenge
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    throw new ArgumentException("A Filename or directory must be specified.");
                }
                var pathName = args[0];
                var horses = from horse in FeedReader.Read(pathName)
                             orderby horse.Price
                             select horse;

                foreach (var horse in horses)
                {
                    Console.Out.WriteLine(
                        $"{horse.Name}, {horse.Price}");
                }
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(
                    "Error caught - " + exception);
            }
        }
    }
}
