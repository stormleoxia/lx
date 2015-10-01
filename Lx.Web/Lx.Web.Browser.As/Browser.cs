#region Copyright (c) 2015 Leoxia Ltd

//  Copyright © 2015 Leoxia Ltd
//  
//  This file is part of Lx.
// 
//  Lx is released under GNU General Public License unless stated otherwise.
//  You may not use this file except in compliance with the License.
//  You can redistribute it and/or modify it under the terms of the GNU General Public License 
//  as published by the Free Software Foundation, either version 3 of the License, 
//  or any later version.
//  
//  In case GNU General Public License is not applicable for your use of Lx, 
//  you can subscribe to commercial license on 
//  http://www.leoxia.com 
//  by contacting us through the form page or send us a mail
//  mailto:contact@leoxia.com
//   
//  Unless required by applicable law or agreed to in writing, 
//  Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
//  OR CONDITIONS OF ANY KIND, either express or implied. 
//  See the GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License along with Lx.
//  It is present in the Lx root folder SolutionItems/GPL.txt
//  If not, see http://www.gnu.org/licenses/.

#endregion

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