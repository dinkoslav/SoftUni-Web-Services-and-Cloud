using Messages.Data.Models;
using Messages.RestServices.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.RestServices.UnitOfWork
{
    interface IUnitOfWork
    {
        IRepository<User> Users { get; }

        IRepository<Channel> Channels { get; }

        IRepository<ChannelMessage> ChannelMessages { get; }

        IRepository<UserMessage> UserMessages { get; }

        void SaveChanges();
    }
}
