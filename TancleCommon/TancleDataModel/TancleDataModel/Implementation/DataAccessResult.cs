using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TancleDataModel.Implementation
{
    public enum ResultCodeOption
    {
        Ok = 0,
        Failure = Ok + 1,
        Duplicate = Failure + 1,
        ValidationFailure = Duplicate + 1,
    }

    public class DataAccessResult
    {
        public static readonly string SuccessDefaultString = "Success";
        public static readonly string FailureDefaultString = "Failure";

        public ResultCodeOption ResultCode;
        public string Message;

        public DataAccessResult(ResultCodeOption resultCode = ResultCodeOption.Failure, string message = "Failure")
        {
            ResultCode = resultCode;
            Message = message;
        }
    }
}
