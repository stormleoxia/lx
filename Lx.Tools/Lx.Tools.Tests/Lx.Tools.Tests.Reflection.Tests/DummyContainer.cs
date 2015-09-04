namespace Lx.Tools.Tests.Reflection.Tests
{
    public class DummyContainer
    {
        private readonly IInterface _instance = new Implementation();
        private readonly string _myField = "FieldValue1";

        public string MyField
        {
            get { return _myField; }
        }

        public IInterface Instance
        {
            get { return _instance; }
        }
    }

    public class Implementation : IInterface
    {
    }

    public interface IInterface
    {
    }
}