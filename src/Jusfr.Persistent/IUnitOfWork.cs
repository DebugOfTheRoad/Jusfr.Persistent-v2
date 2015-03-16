﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jusfr.Persistent {
    public interface IUnitOfWork {
        void Begin();
        void Rollback();
        void Commit();
    }
}
