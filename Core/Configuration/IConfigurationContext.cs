
namespace Core.Configuration
{
    public interface IConfigurationContext
    {
        ConfigurationItem GetItem(string key);
        ConfigurationItems GetAll();
        void SaveItem(string key, string value);
    }
}
