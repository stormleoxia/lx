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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml;
using HtmlAgilityPack;

namespace Lx.Samples.Web.Scraper
{
    /// <summary>
    ///     Curl like tool. Get Static Html and Get a part of it with XPath search.
    ///     However since it doesn't handle Javascript, it's not relevant for all scrape scenario.
    /// </summary>
    public class Program
    {
        private static readonly string _file = "file.html";

        private static void Main(string[] args)
        {
            var url = "http://www.abcbourse.com/graphes/display.aspx?s=PX1p";
            var web = new HtmlWeb();
            web.PreRequest = OnPreRequest;
            var watch = new Stopwatch();
            watch.Start();
            HtmlDocument doc;
            if (!File.Exists(_file))
            {
                doc = web.Load(url);
                doc.Save(_file);
            }
            else
            {
                doc = new HtmlDocument();
                doc.Load(_file);
            }
            var navigator = doc.CreateNavigator();
            var nsmgr = new XmlNamespaceManager(navigator.NameTable);
            nsmgr.AddNamespace("x", "http://www.w3.org/1999/xhtml");
            var selector = "id('vZone')/b[1]";
            var node = navigator.SelectSingleNode(selector);
            Console.WriteLine("Node Selected: {0}", node != null ? node.InnerXml : "NULL");
            watch.Stop();
            Console.WriteLine("Loaded in {0}", watch.Elapsed);
            Console.ReadLine();
        }

        private static bool OnPreRequest(HttpWebRequest request)
        {
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return true;
        }
    }
}