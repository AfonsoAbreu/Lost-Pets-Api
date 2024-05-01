using Application.Facades;
using Application.Facades.Interfaces;
using Infrastructure.Data;

namespace Application.Services.Base
{
    public abstract class BaseService(ApplicationDbContext applicationDbContext) : IDisposable
    {
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
        private ActiveDbTransactionFacade? activeTransaction;

        protected IActiveDbTransactionFacade BeginTransaction()
        {
            activeTransaction?.Dispose();
            activeTransaction = new ActiveDbTransactionFacade(_applicationDbContext.Database.BeginTransaction());
            return activeTransaction;
        }

        public void Dispose()
        {
            activeTransaction?.Dispose();
            GC.SuppressFinalize(this);
        }

        protected void SaveChanges()
        {
            _applicationDbContext.SaveChanges();
        }
    }
}
