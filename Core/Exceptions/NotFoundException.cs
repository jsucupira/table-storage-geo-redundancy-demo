using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base($"{message} was not found.")
        {
            
        }
    }

    [Serializable]
    public class InvalidValueException : Exception
    {
        public InvalidValueException(string name, string value)
            : base($"Invalid value for {name} '{value}'.")
        {
        }
    }
}
