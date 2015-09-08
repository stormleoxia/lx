using System;
using System.Collections.Generic;
using System.Threading;
using Awesomium.Core;
using HtmlAgilityPack;
using Lx.Tools.Common.Threading;
using Lx.Web.Common;

namespace Lx.Web.Browser.Asm
{
    public sealed class Browser : IBrowser
    {
        private readonly List<ManualResetEvent> _tokens = new List<ManualResetEvent>();
        private readonly object _syncRoot = new object();

        public Browser()
        {
        }

        public HtmlDocument Load(string url)
        {
            var token = CreateNewToken();
            var doc = new ThreadSafe<HtmlDocument>();
            ThreadPool.QueueUserWorkItem(x =>
            {
                using (var webView = WebCore.CreateWebView(800, 600))
                {
                    webView.Source = new Uri(url);
                    var view = webView;
                    webView.LoadingFrameComplete += (s, e) =>
                    {
                        if (!e.IsMainFrame)
                            return;

                        doc.Execute(y => y.LoadHtml(view.HTML));

                        token.Set();
                    };
                    WebCore.Run();
                }
            });
            token.WaitOne(TimeSpan.FromSeconds(10));
            return doc.InnerInstance;
        }

        private ManualResetEvent CreateNewToken()
        {
            lock (_syncRoot)
            {
                var mre = new ManualResetEvent(false);
                _tokens.Add(mre);
                return mre;
            }
        }

        public void Dispose()
        {
            lock (_syncRoot)
            {
                foreach (var token in _tokens)
                {
                    token.WaitOne(TimeSpan.FromSeconds(2));
                }
            }
            WebCore.Shutdown();
        }
    }
}
