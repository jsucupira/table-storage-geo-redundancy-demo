using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;

namespace Core.Extensibility
{
    public static class MefBase
    {
        public static ExportProvider Container { get; set; }

        public static T Resolve<T>()
        {
            return Container.ResolveExportedValue<T>();
        }

        public static T Resolve<T>(string className)
        {
            return Container.ResolveExportedValue<T>(className);
        }

        public static IEnumerable<T> ResolveMany<T>()
        {
            return Container.ResolveExportedValues<T>();
        }
    }
}