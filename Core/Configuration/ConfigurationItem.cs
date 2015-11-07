using System.Collections.Generic;

namespace Core.Configuration
{
    public class ConfigurationItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ConfigurationItems : List<ConfigurationItem> { }
}
