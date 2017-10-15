﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SigmaAWS.NetCore2.DAL.Context;
//using SigmaAWS.NetCore2.DAL.Context;
using SigmaAWS.NetCore2.Domain.Interfaces;

namespace SigmaAWS.NetCore2.DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly CustomersContext Db;
        protected readonly DbSet<TEntity> DbSet;
        protected readonly ILogger Logger;

        public Repository(CustomersContext context, ILoggerFactory loggerFactory)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
            Logger = loggerFactory.CreateLogger("CustomerRepository");
        }

        public virtual void Add(TEntity obj)
        {
            DbSet.Add(obj);
        }

        public virtual TEntity GetById(Guid id)
        {
            return DbSet.Find(id);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public virtual void Update(TEntity obj)
        {
            DbSet.Update(obj);
        }

        public virtual void Remove(Guid id)
        {
            DbSet.Remove(DbSet.Find(id));
        }

        public int SaveChanges()
        {
            return Db.SaveChanges();
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
