namespace DQF.Platform.Dispatching.Interfaces
{
    public interface IMessageHandlerInterceptor
    {
        void Intercept(DispatcherInvocationContext context);
    }
}
