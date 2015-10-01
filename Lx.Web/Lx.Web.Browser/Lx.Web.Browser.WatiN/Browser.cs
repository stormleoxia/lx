using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lx.Web.Common;
using mshtml;
using WatiN.Core;
using WatiN.Core.Exceptions;

namespace Lx.Web.Browser.WatiN
{
    public class Browser : IBrowser
    {
        private readonly IE _browser;

        public Browser()
        {
            _browser = new global::WatiN.Core.IE();
        }

        public string CurrentPage
        {
            get { return _browser.Text; }
        }

        public void Dispose()
        {
            _browser.Dispose();
        }

        public string Load(string url)
        {
            _browser.GoTo(url);
            return _browser.Text;
        }

        public void Login(string user, string password)
        {
            var login = _browser.TextField(x => x.Id == "login-email");
            login.Value = user;
            var pass = _browser.TextField(x => x.Id == "login-password");
            pass.Click();
            pass.TypeText(password);
            var button = _browser.Button(x => x.Name == "submit");            
            button.Click();
        }

        public string Interpret(string source)
        {
            return source;
        }
    }
}
