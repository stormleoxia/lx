using System;
using System.Linq;
using AngleSharp;
using AngleSharp.Parser.Html;
using Lx.Web.Common;

namespace Lx.Web.Browser.As
{
    public sealed class Browser : IBrowser
    {
        private readonly IConfiguration _config;
        private readonly HtmlParser _parser;

        public Browser()
        {
            _config = Configuration.Default.WithDefaultLoader().WithJavaScript().WithCookies().WithCss();
            _parser = new HtmlParser(_config);

        }

        public string Load(string url)
        {
            var browsingContext = BrowsingContext.New(_config);
            string result = null;
            using (var task = browsingContext.OpenAsync(new Url(url)))
            {
                if (task.Wait(TimeSpan.FromSeconds(10)))
                {
                    using (var document = task.Result)
                    {
                        result = document.DocumentElement.OuterHtml;
                    }
                }
            }
            return result;
        }

        public string Interpret(string source)
        {
            using (var document = _parser.Parse(source))
            {
                return document.DocumentElement.OuterHtml;
            }
        }

        public void Dispose()
        {
            
        }
    }
}
