namespace Lx.Tools.Performance
{
    public interface IBenchmark
    {
        string Name { get; }
        void Init();
        void Call();
        void Cleanup();
    }
}