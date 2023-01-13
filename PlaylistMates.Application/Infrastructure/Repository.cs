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
        protected DbContext _context;

        public Repository(DbContext context)
        {
            _entities = context.Set<TEntity>();
            _context = context;
        }

        public virtual void InsertOne(TEntity entity)
        {
            _entities.Add(entity);
            _context.SaveChanges();
        }

        public virtual void DeleteOne(TKey id)
        {
            var entity = _entities.Find(id);
            if (entity != null)
            {
                _entities.Remove(entity);
            }

            _context.SaveChanges();
        }

        public virtual void UpdateOne(TEntity entity)
        {
            _entities.Update(entity);
            _context.SaveChanges();
        } 

        public virtual TEntity? FindById(TKey id) => _entities.Find(id);

        public virtual void DeleteAll()
        {
            _entities.RemoveRange(_entities);
            _context.SaveChanges();
        } 
    }

}
