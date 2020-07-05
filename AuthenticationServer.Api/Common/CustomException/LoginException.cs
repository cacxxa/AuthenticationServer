using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Api.Common.CustomException
{
    public class LoginException : Exception
    {
        public LoginException()
        {

        }

        public LoginException(string message) : base(message)
        {
        }

        public LoginException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
