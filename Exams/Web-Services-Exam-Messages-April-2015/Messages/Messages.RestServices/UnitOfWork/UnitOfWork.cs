using Messages.Data;
using Messages.Data.Models;
using Messages.RestServices.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Messages.RestServices.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext dbContext;
        private readonly IDictionary<Type, object> repositories;

        public UnitOfWork()
            :this(new MessagesDbContext())
        {
            
        }

        public UnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<User> Users
        {
            get { return this.GetRepository<User>(); }
        }

        public IRepository<Channel> Channels
        {
            get { return this.GetRepository<Channel>(); }
        }

        public IRepository<ChannelMessage> ChannelMessages
        {
            get { return this.GetRepository<ChannelMessage>(); }
        }

        public IRepository<UserMessage> UserMessages
        {
            get { return this.GetRepository<UserMessage>(); }
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof (GenericEFRepository<T>);
                this.repositories.Add(typeof (T),
                    Activator.CreateInstance(type, this.dbContext));
            }

            return (IRepository<T>) this.repositories[typeof (T)];
        }
    }
}