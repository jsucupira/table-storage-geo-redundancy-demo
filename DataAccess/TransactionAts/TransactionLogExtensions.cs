﻿using Model.Transaction;

namespace DataAccess.TransactionAts
{
    internal static class TransactionLogExtensions
    {
        public static TransactionLogAtsEntity Map(this TransactionLog model)
        {
            if (model == null) return null;
            return new TransactionLogAtsEntity
            {
                RowKey = model.TransactionId,
                Action = model.Action,
                Object = model.Object,
                PartitionKey = model.Type,
                ObjectId = model.ObjectId
            };
        }

        public static TransactionLog Map(this TransactionLogAtsEntity model)
        {
            if (model == null) return null;
            return new TransactionLog
            {
                TransactionId = model.RowKey,
                Action = model.Action,
                Object = model.Object,
                Type = model.PartitionKey,
                ObjectId = model.ObjectId
            };
        }
    }
}