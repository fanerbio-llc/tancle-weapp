using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TancleDataModel.Interface
{
    public interface IDataValidatable
    {
        ValidationResults Validate();
    }
}
