namespace Application.Facades.Interfaces
{
    public interface IActiveDbTransactionFacade: IDisposable
    {
        void Commit();
        void Rollback();
    }
}
