using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Lx.Tools.Db.Proto;

namespace Lx.Tools.Db.Credentials
{
    public class CredentialStore
    {
        private LxDb _db;
        private const string defaultApplication = "Lx.Tools";
        private const string defaultFileName = @"Credentials";

        static CredentialStore()
        {
            Default = new CredentialStore(DefaultLocation);            
        }

        public CredentialStore(string location)
        {
            _db = new LxDb(location);
        }

        public void Save(ICredentials credentials)
        {
            if (credentials != null)
            {
                using (var session = _db.OpenSession())
                {
                    EncryptedCredentials encryptedCredentials = Encrypt(credentials);
                    session.Save<EncryptedCredentials>(credentials.Key, encryptedCredentials);
                }
            }
        }

        private EncryptedCredentials Encrypt(ICredentials credentials)
        {
            return new EncryptedCredentials
            {
                Key = Encrypt(credentials.Key),
                User = Encrypt(credentials.User),
                Password = Encrypt(credentials.Password)
            };
        }

        public static string Encrypt(string toEncrypt)
        {
            var array = ProtectedData.Protect(Encoding.UTF8.GetBytes(toEncrypt), null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(array, Base64FormattingOptions.None);
        }

        public ICredentials Get(string key)
        {
            using (var session = _db.OpenSession())
            {
                var encrypted = session.Get<EncryptedCredentials>(key);
                if (encrypted != null)
                {
                    return Decrypt(encrypted);
                }                
            }
            return null;
        }

        private ICredentials Decrypt(EncryptedCredentials encrypted)
        {
            return new Credentials
            {
                Key = Decrypt(encrypted.Key),
                User = Decrypt(encrypted.User),
                Password = Decrypt(encrypted.Password)
            };
        }

        public static string Decrypt(string encrypted)
        {
            var base64 = Convert.FromBase64String(encrypted);
            var array = ProtectedData.Unprotect(base64, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(array);
        }

        public static string DefaultLocation
        {
            get
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData,
                    Environment.SpecialFolderOption.Create);
                return Path.Combine(appDataPath, defaultApplication, defaultFileName);
            }
        }

        public static CredentialStore Default { get; private set; }

        public ICredentials GetAndAsk(string key)
        {
            var credentials = Get(key);
            if (credentials == null)
            {
                var cred = new Credentials();
                cred.Key = key;
                Console.WriteLine("Please give your user name for {0}", key);
                cred.User = Console.ReadLine();
                Console.WriteLine("Please give your password for {0}", key);
                cred.Password = Console.ReadLine();
                Save(cred);
                return cred;
            }
            return credentials;
        }
    }

    internal class EncryptedCredentials : Credentials
    {
    }

    internal class Credentials : ICredentials
    {
        public string Key { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }

    public interface ICredentials
    {
        string Key { get; }
        string User { get; }
        string Password { get; }
    }
}
