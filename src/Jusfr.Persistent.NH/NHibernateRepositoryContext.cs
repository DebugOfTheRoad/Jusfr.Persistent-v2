﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;
using System.Diagnostics;
using System.Threading;

namespace Jusfr.Persistent.NH {
    public class NHibernateRepositoryContext : DisposableObject, IRepositoryContext {
        private static Int32 _count = 0;
        private readonly Guid _id = Guid.NewGuid();
        private Boolean _suspendTransaction = false;
        private readonly ISessionFactory _sessionFactory;
        private ISession _session;

        public Guid ID {
            get { return _id; }
        }

        public NHibernateRepositoryContext(ISessionFactory sessionFactory) {
            _sessionFactory = sessionFactory;
        }

        protected override void DisposeManaged() {
            if (_session != null) {
                Commit();
            }
        }

        public virtual ISession EnsureSession() {
            if (_session == null) {
                Debug.WriteLine("Open session, count {0}", Interlocked.Increment(ref _count));
                _session = _sessionFactory.OpenSession();
            }
            if (_suspendTransaction && !_session.Transaction.IsActive) {
                Debug.WriteLine("Begin transaction");
                _session.BeginTransaction();
            }
            return _session;
        }

        //仅在Session对象已创建, 但事务未创建或已提交的情况下开启新事务
        public virtual void Begin() {
            _suspendTransaction = true;
            if (_session != null && !_session.Transaction.IsActive) {
                _session.BeginTransaction();
            }
        }

        //仅在事务已创建且处于活动中时回滚事务
        public virtual void Rollback() {
            if (_session != null && _session.Transaction.IsActive) {
                _session.Transaction.Rollback();
                _session.Clear();
            }
        }

        //仅在事务已创建且处于活动中时提交事务
        public virtual void Commit() {
            if (_session != null) {
                try {
                    if (_session.Transaction.IsActive) {
                        _session.Transaction.Commit();
                    }
                }
                catch {
                    if (_session.Transaction.IsActive) {
                        _session.Transaction.Rollback();
                    }
                    _session.Clear();
                    throw;
                }
                finally {
                    Debug.WriteLine("Dispose session, left {0}", Interlocked.Decrement(ref _count));
                    if (_session.Transaction.IsActive) {
                        _session.Transaction.Dispose();
                    }
                    _session.Close();
                    _session.Dispose();
                    _session = null;
                }
            }
        }

        public virtual IQueryable<TEntry> Of<TEntry>() {
            return new NhQueryable<TEntry>(EnsureSession().GetSessionImplementation());
        }
    }
}
