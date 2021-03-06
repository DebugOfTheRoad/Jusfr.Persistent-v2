﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jusfr.Persistent {
    public abstract class Repository<TEntry> : IRepository<TEntry, TEntry> where TEntry : class, IAggregate {
        private IRepositoryContext _context;
        public IRepositoryContext Context {
            get { return _context; }
        }

        public Repository(IRepositoryContext context) {
            _context = context;
        }

        public abstract IQueryable<TEntry> All { get; }
        public abstract Boolean Any(params Expression<Func<TEntry, Boolean>>[] predicates);
        public abstract TEntry Retrive(Int32 id);
        public abstract IEnumerable<TEntry> Retrive<TKey>(String field, IList<TKey> keys);

        public abstract void Create(TEntry entry);
        public abstract void Update(TEntry entry);
        public abstract void Update(IEnumerable<TEntry> entries);
        public abstract void Save(TEntry entry);
        public abstract void Delete(TEntry entry);
        public abstract void Delete(IEnumerable<TEntry> entries);
    }
}
