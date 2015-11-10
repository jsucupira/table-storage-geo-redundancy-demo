using System.ComponentModel.Composition.Hosting;

namespace Core.Extensibility
{
    public static class MefBase
    {
        public static ExportProvider Container { get; set; }

        public static T Resolve<T>()
        {
            return Container.GetExportedValue<T>();
        }

        public static T Resolve<T>(string className)
        {
            return Container.ResolveExportedValue<T>(className);
        }
    }
}