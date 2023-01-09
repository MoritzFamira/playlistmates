using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaylistMates.Application.Model;

namespace PlaylistMates.Application.Infrastructure
{
    public class Repository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        protected readonly DbSet<TEntity> _entities;

        public IQueryable<TEntity> Queryable => _entities.AsQueryable();

        public Repository(DbContext context)
        {
            _entities = context.Set<TEntity>();
        }

        public virtual void InsertOne(TEntity entity) => _entities.Add(entity);

        public virtual void DeleteOne(TKey id)
        {
            var entity = _entities.Find(id);
            if (entity != null)
            {
                _entities.Remove(entity);
            }
        }

        public virtual void UpdateOne(TEntity entity) => _entities.Update(entity);

        public virtual TEntity? FindById(TKey id) => _entities.Find(id);

        public virtual void DeleteAll() => _entities.RemoveRange(_entities);
    }

}
