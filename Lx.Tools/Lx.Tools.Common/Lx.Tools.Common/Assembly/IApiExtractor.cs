using System;

namespace Lx.Tools.Common.Assembly
{
    public interface IApiExtractor : IDisposable
    {
        AssemblyApi ExtractApi(string assemblyPath);
    }
}