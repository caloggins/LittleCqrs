namespace LittleCqrs.Tests;

using Microsoft.Extensions.DependencyInjection;

public class BusTests
{
    private ICqrs sut;
    private string output = "";

    public BusTests()
    {
        var blueListener = new BlueListener(output);
        var redListener = new Func<IServiceProvider, IListener<Alarm>>(output);
        var provider = TestContainer.Create(collection =>
        {
            collection.AddTransient(blueListener);
            collection.AddTransient(redListener);
        });

        sut = provider.GetRequiredService<ICqrs>();
    }
    
    [Fact]
    public async void ItNotifiesAllListeners()
    {
        var alarm = new Alarm();

        await sut.Handle(alarm);
    }
}

public class Alarm : INotification
{
    public string Message = "Oh noes!";
}

public class BlueListener(string output) : IListener<Alarm>
{
    public Task Handle(Alarm notification)
    {
        return null;
    }
}

public class RedListener(string output) : IListener<Alarm>
{
    public Task Handle(Alarm notification)
    {
        return null;
    }
}

interface IListener<TNotification> where TNotification : INotification
{
    Task Handle(TNotification notification);
}