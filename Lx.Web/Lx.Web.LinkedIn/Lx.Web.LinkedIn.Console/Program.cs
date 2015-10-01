using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Db.Credentials;

namespace Lx.Web.LinkedIn.Console
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var program = Lx.Tools.Common.ProgramResolver.Resolve<ConsoleMain>();
/*            program.Arguments.Add(new Argument {Name = "regular_expression"});
            program.Arguments.Add(new Argument {Name = "directory"});
            program.Arguments.Add(new Argument {Name = "[ other_directory ]"});*/
            program.Run(args);
        }
    }

    public class ConsoleMain : ProgramDefinition
    {
        private const string _url = "http://www.linkedin.com";


        public ConsoleMain(Options options, UsageDefinition definition, IEnvironment environment, IDebugger debugger, IConsole console, IVersion versionGetter) 
            : base(options, definition, environment, debugger, console, versionGetter)
        {
        }



        protected override HashSet<Option> InnerManageOptions(HashSet<Option> activatedOptions)
        {
            return activatedOptions;
        }

        protected override void InnerRun(HashSet<Option> options, string[] args)
        {
            using (var browser = new Lx.Web.Browser.WatiN.Browser())
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var res = browser.Load(_url);

                var credentials = CredentialStore.Default.GetAndAsk(_url);
                browser.Login(credentials.User, credentials.Password);
                _console.WriteLine(browser.CurrentPage);
                stopWatch.Stop();
                _console.WriteLine("Done in: " + stopWatch.Elapsed);
                _console.ReadLine();
            }
        }
    }
}
