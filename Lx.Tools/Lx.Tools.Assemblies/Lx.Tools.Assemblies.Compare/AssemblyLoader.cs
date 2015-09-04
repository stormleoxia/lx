using Lx.Tools.Common.Assemblies;

namespace Lx.Tools.Assemblies.Compare
{
    public class AssemblyLoader
    {
        private readonly IApiExtractorFactory _factory;

        public AssemblyLoader(IApiExtractorFactory factory)
        {
            _factory = factory;
        }

        public AssemblyApi ExtractApi(string assemblyPath)
        {
            using (var extractor = _factory.BuildExtractor())
            {
                return extractor.ExtractApi(assemblyPath);
            }
        }
    }

    public interface IApiExtractorFactory
    {
        IApiExtractor BuildExtractor();
    }
}