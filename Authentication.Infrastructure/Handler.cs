using System;

namespace Authentication.Infrastructure
{
    public abstract class Handler
    {
        public IAuthenticationEventRepository EventRepo { get; private set; }


        public Handler(IAuthenticationEventRepository eventRepo)
        {
            if (eventRepo == null)
            {
                throw new ArgumentNullException("eventRepo");
            }

            EventRepo = eventRepo;
        }
    }
}
