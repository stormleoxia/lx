using System;
using Lx.Tools.Common.Assemblies;

namespace Lx.Tools.Assemblies.Compare
{
    public class AppDomainExtractorFactory : IApiExtractorFactory
    {
        public IApiExtractor BuildExtractor()
        {
            var domain = AppDomain.CreateDomain("Loader ");
            var type = typeof (ApiExtractor);
            var containingAssembly = type.Assembly.GetName().Name;
            var apiExtractorTypeName = typeof (ApiExtractor).FullName;
            domain.Load(type.Assembly.GetName());
            var extractor =
                (IApiExtractor) domain.CreateInstanceFromAndUnwrap(containingAssembly + ".dll", apiExtractorTypeName);
            return new DomainDisposableExtractor(domain, extractor);
        }
    }
}