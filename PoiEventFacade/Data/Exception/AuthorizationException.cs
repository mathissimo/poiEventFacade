using System;

namespace PoiEventNetwork.Data.Exception
{
    public class AuthorizationException : System.Exception
    {
        public AuthorizationException(long id)
        {
            Id = id;
        }

        public override string Message
        {
            get { return String.Format("User with id {0} is not authorized for this kind of operation.", Id); }
        }

        private long Id { get; set; }
    }
}
