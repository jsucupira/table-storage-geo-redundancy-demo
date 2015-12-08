using Microsoft.WindowsAzure.Storage.Table;

namespace DataAccess.TransactionAts
{
    internal class TransactionLogAtsEntity : TableEntity
    {
        public string Action { get; set; }
        public string Object { get; set; }
        public string ObjectId { get; set; }
    }
}