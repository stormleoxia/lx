using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml;
using HtmlAgilityPack;

namespace Lx.Samples.Web.Scraper
{
    /// <summary>
    ///     Curl like tool
    /// </summary>
    public class Program
    {
        private static readonly string _file = "file.html";

        private static void Main(string[] args)
        {
            var url =
                "http://www.poderjudicialdf.gob.mx/es/PJDF/Resutados_Boletin?numero=JF01&autoridad=J&submit=consultar&publicacion=02-09-2013&materia=F&s181=11";
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
            for (var row = 0; row < 10; ++row)
            {
                for (var column = 0; column < 20; ++column)
                {
                    var selector = "id('contenidos')/table/tr[" + row + "]/td[" + column + "]";
                    // /x:tbody/x:tr[7]/x:td[2] id('contenidos')/table/tbody/tr[7]/td[2]
                    var node = navigator.SelectSingleNode(selector);
                    Console.WriteLine("Node Selected: {0}", node != null ? node.InnerXml : "NULL");
                }
            }
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