using Core.Configuration;
using Core.Extensibility;

namespace Model.Customer
{
    public static class CustomerContextFactory
    {
        public static ICustomerContext Create()
        {
            var context = MefBase.Resolve<ICustomerContext>();
            context.Redundant();
            return context;
        }

        public static ICustomerContext CreateSimple()
        {
            return MefBase.Resolve<ICustomerContext>();
        }
    }
}