using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Base
{
    public abstract class BaseRepository<T>(ApplicationDbContext context) : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context = context;

        protected DbSet<T> GetSet()
        {
            return _context.Set<T>();
        }

        protected DbSet<T2> GetSet<T2>() where T2 : class
        {
            return _context.Set<T2>();
        }

        protected EntityEntry<T> GetEntry(T entity)
        {
            return _context.Entry(entity);
        }

        public void Add(T entity)
        {
            GetSet().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            GetSet().AddRange(entities);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return GetSet().Where(expression);
        }

        public IEnumerable<T> GetAll()
        {
            return GetSet().AsEnumerable();
        }

        public T? GetById(Guid id)
        {
            return GetSet().Find(id);
        }

        public void Remove(T entity)
        {
            GetSet().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            GetSet().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            GetSet().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            GetSet().UpdateRange(entities);
        }

        public int RemoveWhere(Expression<Func<T, bool>> where)
        {
            return GetSet()
                .Where(where)
                .ExecuteDelete();
        }

        public int UpdateWhere(Expression<Func<T, bool>> where, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> set)
        {
            return GetSet()
                .Where(where)
                .ExecuteUpdate(set);
        }

        public void ExplicitLoadCollection<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression) where TProperty : class
        {
            GetEntry(entity)
                .Collection(propertyExpression)
                .Load();
        }

        public void ExplicitLoadReference<TProperty>(T entity, Expression<Func<T, TProperty?>> propertyExpression) where TProperty : class
        {
            GetEntry(entity)
                .Reference(propertyExpression)
                .Load();
        }

        public void Detach(T entity)
        {
            GetEntry(entity).State = EntityState.Detached;
        }
    }
}
