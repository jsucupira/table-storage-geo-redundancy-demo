using System;
using System.ComponentModel.Composition;

namespace Core.Extensibility
{
    [AttributeUsage(AttributeTargets.Class), MetadataAttribute]
    public class MefExportAttribute : ExportAttribute, INameMetadata
    {
        public MefExportAttribute(Type contractType, string name): base(contractType)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public interface INameMetadata
    {
        string Name { get; }
    }
}
