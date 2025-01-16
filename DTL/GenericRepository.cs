using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TP_CAISSE.Context;

namespace TP_CAISSE.DTL
{
    public delegate void BeforeEntitiesSavedHandler<T>(IEnumerable<T> entities) where T : class;

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public event BeforeEntitiesSavedHandler<T> OnBeforeSaveEntities;

        public MyDbContext Context => _context;

        private readonly MyDbContext _context;

        protected readonly IServiceProvider Provider;

        public DbSet<T> DbSet => _context.Set<T>();

        public GenericRepository(IServiceProvider provider)
        {
            Provider = provider;
            _context = Provider.GetRequiredService<MyDbContext>();
        }

        public virtual IEnumerable<T> GetAll(bool asNoTracking = false)
        {
            if (asNoTracking)
            {
                return DbSet.AsNoTracking().ToList();
            }

            return DbSet.ToList();
        }

        public IQueryable<T> GetQuery(params Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> query = DbSet;

            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

            return query;
        }

        public virtual T GetById(object primaryKey)
        {
            return DbSet.Find(primaryKey)!;
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public virtual void Add(T entity)
        {
            DbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Added;
        }

        public virtual void Update(T entity)
        {
            if (entity != null)
            {
                var existingEntity = DbSet.Local.FirstOrDefault(e => e == entity);
                if (existingEntity == null)
                {
                    DbSet.Attach(entity);
                    _context.Entry(entity).State = EntityState.Modified;
                }
            }
        }

        public virtual void Delete(T entityToDelete)
        {
            DbSet.Remove(entityToDelete);
        }

        public virtual void Delete(object id)
        {
            var entityToDelete = DbSet.Find(id);
            if (entityToDelete != null)
            {
                Delete(entityToDelete);
            }
        }

        public virtual async Task SaveAsync()
        {
            OnBeforeSaveEntities?.Invoke(await DbSet.ToListAsync());
            await _context.SaveChangesAsync();
        }

        public virtual void Save()
        {
            OnBeforeSaveEntities?.Invoke(DbSet.ToList());
            _context.SaveChanges();
        }

        public virtual async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = false)
        {
            if (asNoTracking)
            {
                return await DbSet.AsNoTracking().ToListAsync();
            }

            return await DbSet.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(object primaryKey)
        {
            return await DbSet.FindAsync(primaryKey);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.SingleOrDefaultAsync(predicate);
        }

        IEnumerable<T> IGenericRepository<T>.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
