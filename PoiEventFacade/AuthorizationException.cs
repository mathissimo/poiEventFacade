using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoiEventFacade
{
    class AuthorizationException : Exception
    {
        public AuthorizationException(long id)
        {
            Id = id;
        }

        public override string Message
        {
            get { return String.Format("User with {0} is not authorized for this kind of operation.", Id); }
        }

        public long Id { get; private set; }
    }
}
