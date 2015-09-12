using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Messages.RestServices.Repositories
{
    public class GenericEFRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext dbContext;
        private IDbSet<TEntity> entitySet;

        public GenericEFRepository(DbContext context)
        {
            this.dbContext = context;
            this.entitySet = dbContext.Set<TEntity>();
        }

        public IDbSet<TEntity> EntitySet {
            get { return this.entitySet; }
        }

        public IQueryable<TEntity> All()
        {
            return this.entitySet;
        }

        public TEntity Add(TEntity entity)
        {
            this.ChangeState(entity, EntityState.Added);
            return entity;
        }

        public void Remove(TEntity entity)
        {
            this.ChangeState(entity, EntityState.Deleted);
        }

        public void Update(TEntity entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public TEntity Find(int id)
        {
            return this.entitySet.Find(id);
        }

        private void ChangeState(TEntity entity, EntityState state)
        {
            var entry = this.dbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.dbContext.Set<TEntity>().Attach(entity);
            }

            entry.State = state;
        }
    }
}