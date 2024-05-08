using Infrastructure.Data.Entities.Base;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Base
{
    public interface IBaseRepository<T> where T : IBaseEntity
    {
        T? GetById(Guid id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        int RemoveWhere(Expression<Func<T, bool>> where);
        int UpdateWhere(Expression<Func<T, bool>> where, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> set);
        void ExplicitLoadCollection<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression) where TProperty : class;
        void ExplicitLoadReference<TProperty>(T entity, Expression<Func<T, TProperty?>> propertyExpression) where TProperty : class;
        void Detach(T entity);
        void Attach(T entity);
    }
}
