using Infrastructure.Data;
using Infrastructure.Data.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Linq;

namespace Infrastructure.Repositories.Base
{
    public abstract class BaseRepository<T>(ApplicationDbContext context) : IBaseRepository<T> where T : class, IBaseEntity
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

        public virtual void Add(T entity)
        {
            GetSet().Add(entity);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            GetSet().AddRange(entities);
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return GetSet().Where(expression);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return GetSet().AsEnumerable();
        }

        public virtual T? GetById(Guid id)
        {
            return GetSet().Find(id);
        }

        public virtual void Remove(T entity)
        {
            GetSet().Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            GetSet().RemoveRange(entities);
        }

        public virtual void Update(T entity)
        {
            GetSet().Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            GetSet().UpdateRange(entities);
        }

        public virtual int RemoveWhere(Expression<Func<T, bool>> where)
        {
            Func<T, bool> compiledWhere = where.Compile();

            DetachWhere(compiledWhere);

            return GetSet()
                .Where(where)
                .ExecuteDelete();
        }

        public virtual int UpdateWhere(Expression<Func<T, bool>> where, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> set)
        {
            Func<T, bool> compiledWhere = where.Compile();

            DetachWhere(compiledWhere);

            return GetSet()
                .Where(where)
                .ExecuteUpdate(set);
        }

        public virtual void ExplicitLoadCollection<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression, Func<IQueryable<TProperty>, IQueryable<TProperty>>? queryExpression = null) where TProperty : class
        {
            var query = GetEntry(entity)
                .Collection(propertyExpression)
                .Query();

            if (queryExpression != null)
            {
                query = queryExpression(query);
            }

            query.Load();
        }

        public virtual void ExplicitLoadReference<TProperty>(T entity, Expression<Func<T, TProperty?>> propertyExpression, Func<IQueryable<TProperty>, IQueryable<TProperty>>? queryExpression = null) where TProperty : class
        {
            var query = GetEntry(entity)
                .Reference(propertyExpression)
                .Query();

            if (queryExpression != null)
            {
                query = queryExpression(query);
            }

            query.Load();
        }

        protected virtual void Detach(EntityEntry<T> entry)
        {
            entry.State = EntityState.Detached;
        }

        protected virtual void Attach(EntityEntry<T> entry)
        {
            _context.Attach(entry);
        }

        public virtual void Detach(T entity)
        {
            GetEntry(entity).State = EntityState.Detached;
        }

        public virtual void Attach(T entity)
        {
            _context.Attach(entity);
        }

        public virtual bool IsAttached(T entity)
        {
            var entry = GetEntry(entity);
            return entry.State != EntityState.Detached;
        }

        public virtual void Detach(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Detach(entity);
            }
        }

        protected virtual void DetachWhere(Func<T, bool> where)
        {
            IEnumerable<EntityEntry<T>> entries = _context.ChangeTracker
                .Entries<T>()
                .Where(entry => where(entry.Entity));

            foreach (EntityEntry<T> entry in entries)
            {
                Detach(entry);
            }
        }

        public virtual void Attach(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Attach(entity);
            }
        }
    }
}
