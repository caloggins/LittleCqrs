namespace LittleCqrs;

public interface ICqrs
{
    Task Handle(ICommand command);
    Task<TResponse> Handle<TResponse>(IQuery<TResponse> query) where TResponse : class;
    Task Handle(INotification notification);
}

public class Cqrs(IServiceProvider provider) : ICqrs
{
    public async Task Handle(ICommand command)
    {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

        var handler = provider.GetService(handlerType);

        var handleMethod = handlerType.GetMethod("Handle");
        var task = (Task)handleMethod!.Invoke(handler, [command]);

        await task;
    }
    
    public async Task<TResponse> Handle<TResponse>(IQuery<TResponse> query) where TResponse : class
    {
        var queryType = query.GetType();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResponse));
    
        var handler = provider.GetService(handlerType);
        if (handler is null)
            throw new NoSuchHandlerException();
        
        var handleMethod = handlerType.GetMethod("Handle");
        var task = (Task<TResponse>)handleMethod!.Invoke(handler, [query]);
    
        return await task;
    }
}