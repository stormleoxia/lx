using System;
using System.Threading;
using Lx.Tools.Common.Assemblies;

namespace Lx.Tools.Assemblies.Compare
{
    public sealed class DomainDisposableExtractor : IApiExtractor
    {
        private readonly AppDomain _domain;
        private readonly IApiExtractor _extractor;

        public DomainDisposableExtractor(AppDomain domain, IApiExtractor extractor)
        {
            _domain = domain;
            _extractor = extractor;
        }

        public void Dispose()
        {
            Exception lastException = null;
            try
            {
                _extractor.Dispose();
                AppDomain.Unload(_domain);
            }
            catch (CannotUnloadAppDomainException)
            {
                var i = 0;
                while (i < 3)
                {
                    Thread.Sleep(3000);
                    try
                    {
                        AppDomain.Unload(_domain);
                        lastException = null;
                    }
                    catch (Exception e)
                    {
                        lastException = e;
                        i++;
                        continue;
                    }
                    break;
                }
            }
            if (lastException != null)
            {
                Console.Error.WriteLine(lastException.ToString());
            }
        }

        public AssemblyApi ExtractApi(string assemblyPath)
        {
            return _extractor.ExtractApi(assemblyPath);
        }
    }
}