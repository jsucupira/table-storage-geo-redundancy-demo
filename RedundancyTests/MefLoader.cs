using System.ComponentModel.Composition.Hosting;
using AzureUtilities.Mock;
using Core.Extensibility;
using DataAccess;

namespace RedundancyTests
{
    public class MefLoader
    {
        public static void Init()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (DataAccessAssembly).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (AzureUtilitiesMockAssembly).Assembly));
            MefBase.Container = new CompositionContainer(catalog, true);
        }
    }
}