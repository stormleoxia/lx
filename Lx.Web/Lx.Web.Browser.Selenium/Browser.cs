using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lx.Web.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;

namespace Lx.Web.Browser.Selenium
{
    public class Browser : IBrowser
    {
        private readonly PhantomJSDriver driver;

        public void Dispose()
        {
            driver.Dispose();
        }

        public Browser()
        {
            Proxy proxy = new Proxy();
            proxy.SslProxy = string.Format("127.0.0.1:9998");
            proxy.HttpProxy = string.Format("127.0.0.1:9999");
            var service = PhantomJSDriverService.CreateDefaultService();
            service.ProxyType = "http";
            service.Proxy = proxy.HttpProxy;
            driver = new PhantomJSDriver(service);
        }

        public string Load(string url)
        {
            driver.Navigate().GoToUrl(url);
            return driver.PageSource;
        }

        public void Login(string user, string password)
        {
            driver.FindElementById("session_key-login").SendKeys(user);
            driver.FindElementById("session_password-login").SendKeys(password);
            driver.FindElementById("btn-primary").Click();
        }

        public string CurrentPageSource
        {
            get { return driver.PageSource; }
        }

        public string Interpret(string source)
        {
            return source;
        }
    }
}
