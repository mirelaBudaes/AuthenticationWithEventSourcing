namespace Authentication.Infrastructure
{
    public interface IBus
    {
        void Send<T>(T command) where T : Command;

        void RaiseEvent<T>(T theEvent) where T : AuthenticationEvent;

        void RegisterSaga<T>() where T : Saga;

        void RegisterHandler<T>();
    }
}
