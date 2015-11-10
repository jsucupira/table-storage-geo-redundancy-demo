
using Core.Extensibility;

namespace Model.Archiver
{
    public static class ArchiveContextFactory
    {
        public static IArchiveContext Create(string archiveName)
        {
            return MefBase.Resolve<IArchiveContext>(archiveName);
        }
    }
}