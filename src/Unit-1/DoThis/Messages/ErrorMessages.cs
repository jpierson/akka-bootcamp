using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTail.Messages
{
    public class InputError
    {
        public InputError(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; }
    }

    public class NullInputError : InputError
    {
        public NullInputError(string reason) : base(reason)
        {

        }
    }

    public class ValidationError : InputError
    {
        public ValidationError(string reason) : base(reason)
        {

        }
    }
}
