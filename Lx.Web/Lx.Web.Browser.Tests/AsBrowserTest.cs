using System;
using System.IO;
using NUnit.Framework;

namespace Lx.Web.Browser.Tests
{
    [TestFixture]
    public class AsBrowserTest
    {
        [Test]
        public void InterpretTest()
        {
            As.Browser browser = new As.Browser();
            string payload;
            using (var file = File.OpenText("PageWithScript.html"))
            {
                payload = file.ReadToEnd();
            }
            var res = browser.Interpret(payload);
            Console.WriteLine(res);
        }
    }
}
