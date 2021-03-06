﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jusfr.Persistent {
    public interface IRepository<in TEntry, out TResult> where TEntry : IAggregate {
        void Create(TEntry entry);
        void Update(TEntry entry);
        void Update(IEnumerable<TEntry> entries);
        void Save(TEntry entry);
        void Delete(TEntry entry);
        void Delete(IEnumerable<TEntry> entries);

        IQueryable<TResult> All { get; }
        TResult Retrive(Int32 id);
        IEnumerable<TResult> Retrive<TKey>(String field, IList<TKey> keys);
    }
}
