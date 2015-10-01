using Lx.Tools.Db.Credentials;
using NUnit.Framework;

namespace Lx.Tools.Db.Tests
{
    [TestFixture]
    class CryptoTest
    {
        [Test]
        public void EncryptDecriptIsIdentity()
        {
            CheckDecryptEncrypt("http://www.leoxia.com");
            CheckDecryptEncrypt("é$ç@!§€$¤~)à£µ");
        }

        private void CheckDecryptEncrypt(string str)
        {
            Assert.AreEqual(str, CredentialStore.Decrypt(CredentialStore.Encrypt(str)));
        }
    }
}
