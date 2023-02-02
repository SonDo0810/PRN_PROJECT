using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Project.Entities;

namespace Project.Repositories
{
    public abstract class BaseRepository<T, ID> where T : BaseEntity<ID>
    {
        private static DbContext _dbContext;

        public static void SetDbContext(DbContext context)
        {
            _dbContext = context;
        }

        public T? FindOne(ID id)
        {
            if(id == null)
            {
                throw new ArgumentException("Id should not be null.");
            }
            DbSet<T> context = GetContext();
            return context.First(x => id!.Equals(x.Id));
        }

        public T GetOne(ID id, string message = "Record not found.")
        {
            var entity = FindOne(id);
            if (entity == null)
            {
                throw new ArgumentException(message);
            }
            return entity;
        }

        public List<T> FindAll()
        {
            DbSet<T> context = GetContext();
            return context.ToList();
        }

        public List<T> FindAll(Collection<ID> ids)
        {
            DbSet<T> context = GetContext();
            HashSet<ID> idSet = ids.ToHashSet();
            return context.Where(e => idSet.Contains(e.Id)).ToList();
        }

        private static DbSet<T> GetContext()
        {
            return _dbContext.Set<T>();
        }

        public T Add(T entity)
        {
            DbSet<T> context = GetContext();
            context.Add(entity);
            return entity;
        }

        public List<T> Add(Collection<T> entities)
        {
            DbSet<T> context = GetContext();
            context.AddRange(entities);
            return entities.ToList();
        }

        public T Update(T entity)
        {
            DbSet<T> context = GetContext();
            context.Update(entity);
            return entity;
        }

        public List<T> Update(Collection<T> entities)
        {
            DbSet<T> context = GetContext();
            context.UpdateRange(entities);
            return entities.ToList();
        }

        public void Delete(ID id)
        {
            DbSet<T> context = GetContext();
            context.Remove(GetOne(id));
        }

        public void DeleteRange(Collection<ID> ids)
        {
            DbSet<T> context = GetContext();
            context.RemoveRange(FindAll(ids));
        }

    }
}
