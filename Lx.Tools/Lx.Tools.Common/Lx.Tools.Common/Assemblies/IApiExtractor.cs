using System;

namespace Lx.Tools.Common.Assemblies
{
    public interface IApiExtractor : IDisposable
    {
        AssemblyApi ExtractApi(string assemblyPath);
    }
}