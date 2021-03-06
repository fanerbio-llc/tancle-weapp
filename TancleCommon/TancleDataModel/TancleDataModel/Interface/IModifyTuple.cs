﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TancleDataModel.Implementation;

namespace TancleDataModel.Interface
{
    public interface IModifyTuple<TEntity> where TEntity : class
    {
        DataAccessResult Modify(TEntity entity);
    }
}
