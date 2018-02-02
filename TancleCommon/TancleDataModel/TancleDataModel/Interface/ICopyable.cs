using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TancleDataModel.Interface
{
    public interface ICopyable<TEntity> where TEntity: class
    {
        void CopyTo(TEntity destination);
    }
}
