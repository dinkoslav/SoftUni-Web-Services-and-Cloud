using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Messages.RestServices.Repositories
{
    public interface IRepository<T>
    {
        T Add(T entity);

        T Find(int id);

        IQueryable<T> All();

        void Remove(T entity);

        void Update(T entity);

        void SaveChanges();
    }
}