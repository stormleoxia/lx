
using System;
using System.Collections.Generic;
using System.Threading;
using Awesomium.Core;
using HtmlAgilityPack;

namespace Lx.Samples.Web.Awesomium
{
    public class Browser : IBrowser
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

                        //BitmapSurface surface = (BitmapSurface) webView.Surface;
                        //surface.SaveToPNG("result.png", true);

                        token.Set();
                    };
                    WebCore.Run();
                }
            });
            token.WaitOne(TimeSpan.FromSeconds(5));
            return doc.InnerInstance;
        }

        private void StartView(object state)
        {

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

    internal class ThreadSafe<T> where T:new()
    {
        private readonly object _syncRoot = new object();
        private readonly T _instance;

        public ThreadSafe()
        {
            _instance = new T();
        }

        public T InnerInstance
        {
            get
            {
                lock (_syncRoot)
                {
                    return _instance;
                }
            }
        }

        public void Execute(Action<T> action)
        {
            lock (_syncRoot)
            {
                action(_instance);
            }
        }
    }

    internal interface IBrowser : IDisposable
    {
        HtmlDocument Load(string url);
    }
}
