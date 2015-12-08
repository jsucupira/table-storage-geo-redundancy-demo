
using Core.Extensibility;

namespace Model.Transaction
{
    public static class TransactionLogContextFactory
    {
        public static ITransactionLogContext Create(string archiveName)
        {
            return MefBase.Resolve<ITransactionLogContext>(archiveName);
        }
    }
}