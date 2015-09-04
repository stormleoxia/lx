namespace Lx.Tools.Common.Wrappers
{
    public interface IEnvironment
    {
        string NewLine { get; }
        void Exit(int exitCode);
    }
}