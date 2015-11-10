using Model.Archiver;

namespace DataAccess.ArchiverAts
{
    internal static class ArchiveExtensions
    {
        public static ArchiveAtsEntity Map(this Archive model)
        {
            if (model == null) return null;
            return new ArchiveAtsEntity
            {
                RowKey = model.ArchiveId,
                Action = model.Action,
                Object = model.Object,
                PartitionKey = model.Type
            };
        }

        public static Archive Map(this ArchiveAtsEntity model)
        {
            if (model == null) return null;
            return new Archive
            {
                ArchiveId = model.RowKey,
                Action = model.Action,
                Object = model.Object,
                Type = model.PartitionKey
            };
        }
    }
}