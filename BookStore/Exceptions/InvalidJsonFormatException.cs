using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Exceptions
{
    public class InvalidJsonFormatException : Exception
    {
        public IEnumerable<ValidationError> ValidationErrors { get;}

        public InvalidJsonFormatException(IEnumerable<ValidationError> errors)
        {
            ValidationErrors = errors;
        }
    }
}
