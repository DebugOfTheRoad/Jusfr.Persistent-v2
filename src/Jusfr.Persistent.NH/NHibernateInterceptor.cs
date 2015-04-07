﻿using NHibernate;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Jusfr.Persistent.NH {
    public class NHibernateInterceptor : EmptyInterceptor, IInterceptor {
        public override SqlString OnPrepareStatement(SqlString sql) {
            Debug.WriteLine(sql);
            return base.OnPrepareStatement(sql);
        }
    }
}
