using System;

namespace Model.Transaction
{
    [Serializable]
    public class TransactionLog
    {
        public TransactionLog()
        {
            TransactionId = Guid.NewGuid().ToString();
        }

        public string Action { get; set; }
        public string TransactionId { get; set; }
        public string ObjectId { get; set; }
        public string Object { get; set; }
        public string Type { get; set; }
    }
}