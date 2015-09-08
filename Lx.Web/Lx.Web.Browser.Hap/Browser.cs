using System.Net;
using HtmlAgilityPack;
using Lx.Web.Common;

namespace Lx.Web.Browser.Hap
{
    public sealed class Browser : IBrowser
    {
        private readonly HtmlWeb _web;

        public Browser()
        {
            _web = new HtmlWeb();
            _web.PreRequest = OnPreRequest;
        }

        public HtmlDocument Load(string url)
        {
            return _web.Load(url);
        }

        private static bool OnPreRequest(HttpWebRequest request)
        {
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return true;
        }

        public void Dispose()
        {
            _web.PreRequest = null;
        }
    }
}
