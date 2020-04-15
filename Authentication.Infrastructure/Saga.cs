using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Infrastructure
{
    public abstract class Saga
    {
        public IBus Bus { get; private set; }
        public IAuthenticationEventRepository EventStore { get; private set; }


        protected Saga(IBus bus, IAuthenticationEventRepository eventStore)
        {
            if (bus == null)
            {
                throw new ArgumentNullException("bus");
            }
            //if (eventStore == null)
            //{
            //    throw new ArgumentNullException("eventStore");
            //}

            Bus = bus;
            EventStore = eventStore;
        }
    }
}
