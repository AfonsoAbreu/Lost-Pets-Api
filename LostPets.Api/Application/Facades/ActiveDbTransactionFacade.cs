using Application.Facades.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Facades
{
    public class ActiveDbTransactionFacade(IDbContextTransaction dbContextTransaction) : IActiveDbTransactionFacade
    {
        protected readonly IDbContextTransaction _dbContextTransaction = dbContextTransaction;

        public void Commit()
        {
            _dbContextTransaction.Commit();
        }

        public void Dispose()
        {
            _dbContextTransaction.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Rollback()
        {
            _dbContextTransaction.Rollback();
        }
    }
}
