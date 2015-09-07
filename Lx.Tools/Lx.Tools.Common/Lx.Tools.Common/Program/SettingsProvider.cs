using System.Configuration;

namespace Lx.Tools.Common.Program
{
    public class SettingsProvider : ISettingsProvider
    {
        public string GetSettings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }

    public interface ISettingsProvider
    {
        string GetSettings(string mysettings);
    }
}
