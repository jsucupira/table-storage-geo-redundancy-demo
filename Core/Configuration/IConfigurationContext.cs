
using System.Collections.Generic;

namespace Core.Configuration
{
    public interface IConfigurationContext
    {
        ConfigurationItem GetItem(string key);
        List<ConfigurationItem> GetAll();
        void SaveItem(string key, string value);
    }
}
