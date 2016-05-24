using System.ComponentModel.Composition.Hosting;
using Azure.TableStorage.Utilities;
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
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (AzureTableStorageUtilitiesAssembly).Assembly));
            MefBase.Container = new CompositionContainer(catalog);
            Azure.TableStorage.Redundancy.MefBase.Container = new CompositionContainer(catalog);
        }
    }
}