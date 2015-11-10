using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace Core.Extensibility
{
    public static class MefExtensionMethods
    {
        public static T ResolveExportedValue<T>(this ExportProvider container, string name)
        {
            if (container == null)
                throw new Exception("MEF composition container is null.");

            T export = container.GetExports<T, INameMetadata>().Where(t => t.Metadata.Name.ToString().Equals(name)).Select(t => t.Value).FirstOrDefault();
            if (export == null)
                throw new Exception($"Could not resolve MEF Export for '{typeof (T).Name}' with the name {name}.");

            return export;
        }
    }
}