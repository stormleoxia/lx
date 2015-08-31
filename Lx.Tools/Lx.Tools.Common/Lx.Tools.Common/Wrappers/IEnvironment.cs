namespace Lx.Tools.Common.Wrappers
{
    public interface IEnvironment
    {
        void Exit(int exitCode);
        string NewLine { get; }
    }
}