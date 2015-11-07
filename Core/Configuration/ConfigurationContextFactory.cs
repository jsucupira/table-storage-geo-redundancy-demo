using Core.Extensibility;

namespace Core.Configuration
{
    public class ConfigurationContextFactory
    {
        public static IConfigurationContext Create()
        {
            return MefBase.Resolve<IConfigurationContext>();
        }
    }
}