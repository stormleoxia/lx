using Lx.Web.Common;

namespace Lx.Web.Browser
{
    class Program
    {
        static void Main(string[] args)
        {
            IBrowser browser = new Hap.Browser();
            var doc = browser.Load("http://www.google.com");
        }
    }
}
