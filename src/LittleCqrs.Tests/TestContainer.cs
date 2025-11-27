namespace LittleCqrs.Tests;

using Microsoft.Extensions.DependencyInjection;

public class TestContainer
{
    public static IServiceProvider Create(Action<ServiceCollection> collection)
    {
        var services = new ServiceCollection();
        collection(services);
        services.AddSingleton<ICqrs, Cqrs>();
        
        var provider = services.BuildServiceProvider();
        return provider;
    }
}