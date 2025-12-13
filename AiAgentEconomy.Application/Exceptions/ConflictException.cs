using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiAgentEconomy.Application.Exceptions
{
    public sealed class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
