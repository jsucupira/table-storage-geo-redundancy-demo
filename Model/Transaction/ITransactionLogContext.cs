using System.Collections.Generic;

namespace Model.Transaction
{
    public interface ITransactionLogContext
    {
        List<TransactionLog> FindAll();
        TransactionLog Save(TransactionLog transactionLog);
    }
}