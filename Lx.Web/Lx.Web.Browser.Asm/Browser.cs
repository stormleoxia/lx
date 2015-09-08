#region Copyright (c) 2015 Leoxia Ltd

// #region Copyright (c) 2015 Leoxia Ltd
// 
// // Copyright © 2015 Leoxia Ltd.
// // 
// // This file is part of Lx.
// //
// // Lx is released under GNU General Public License unless stated otherwise.
// // You may not use this file except in compliance with the License.
// // You can redistribute it and/or modify it under the terms of the GNU General Public License 
// // as published by the Free Software Foundation, either version 3 of the License, 
// // or any later version.
// // 
// // In case GNU General Public License is not applicable for your use of Lx, 
// // you can subscribe to commercial license on 
// // http://www.leoxia.com 
// // by contacting us through the form page or send us a mail
// // mailto:contact@leoxia.com
// //  
// // Unless required by applicable law or agreed to in writing, 
// // Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
// // OR CONDITIONS OF ANY KIND, either express or implied. 
// // See the GNU General Public License for more details.
// //
// // You should have received a copy of the GNU General Public License along with Lx.
// // It is present in the Lx root folder SolutionItems/GPL.txt
// // If not, see http://www.gnu.org/licenses/.
// //
// 
// #endregion

#endregion

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
        private readonly object _syncRoot = new object();
        private readonly List<ManualResetEvent> _tokens = new List<ManualResetEvent>();

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

        private ManualResetEvent CreateNewToken()
        {
            lock (_syncRoot)
            {
                var mre = new ManualResetEvent(false);
                _tokens.Add(mre);
                return mre;
            }
        }
    }
}