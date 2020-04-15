namespace Authentication.Infrastructure
{
    public interface IHandleMessage<in T>
    {
        void Handle(T authenticationEvent);
    }
}
