using System.ComponentModel.Composition.Hosting;
using AzureUtilities;
using Core.Extensibility;
using DataAccess;

namespace Services.BootStrapper
{
    public class MefLoader
    {
        public static void Initialize()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (DataAccessAssembly).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (AzureUtilitiesAssembly).Assembly));
            MefBase.Container = new CompositionContainer(catalog);
            Azure.TableStorage.Redundancy.MefBase.Container = new CompositionContainer(catalog);
        }
    }
}