using Microsoft.WindowsAzure.Storage.Table;

namespace DataAccess.ArchiverAts
{
    internal class ArchiveAtsEntity : TableEntity
    {
        public string Action { get; set; }
        public string Object { get; set; }
    }
}