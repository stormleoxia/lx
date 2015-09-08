using System;
using HtmlAgilityPack;

namespace Lx.Web.Common
{
    public interface IBrowser : IDisposable
    {
        HtmlDocument Load(string url);
    }
}
