using System;
using System.Diagnostics;

namespace Lx.Samples.Web.Browser
{
    /// <summary>
    /// Should Handle all scrape scenario (javascript included)
    /// </summary>
    class Program
    {


        static void Main(string[] args)
        {
            try
            {
                string url = @"http://www.google.com";
                var browser = new Awesomium.Browser();
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var document = browser.Load(url);
                stopwatch.Stop();
                Console.WriteLine(document.DocumentNode.InnerText);
                Console.WriteLine("Loaded in " + stopwatch.Elapsed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadLine();
        }
    }
}
