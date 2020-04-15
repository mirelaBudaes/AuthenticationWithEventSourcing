using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Infrastructure
{
    public interface IStartWithMessage<in T> where T : Message
    {
        void Handle(T message);
    }
}
