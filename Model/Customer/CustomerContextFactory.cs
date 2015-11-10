using Core.Configuration;
using Core.Extensibility;

namespace Model.Customer
{
    public static class CustomerContextFactory
    {
        public static ICustomerContext Create()
        {
            string className = ConfigurationsSelector.GetSetting("Customer.DataAccess", "CustomerAtsContext");
            return MefBase.Resolve<ICustomerContext>(className);
        }

        public static ICustomerContext CreateSimple()
        {
            return MefBase.Resolve<ICustomerContext>("CustomerAtsContext");
        }
    }
}