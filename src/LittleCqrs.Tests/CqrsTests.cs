using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace LittleCqrs.Tests;

using FakeItEasy;

public class CqrsTests
{
    private readonly ICqrs sut;
    private readonly ICommandHandler<DoIt> commandHandler = A.Fake<ICommandHandler<DoIt>>();
    
    public CqrsTests()
    {
        var provider = TestContainer.Create(collection =>
        {
            collection.AddSingleton(commandHandler);
            collection.AddTransient<IQueryHandler<Ping, string>, PingHandler>();
        });
        sut = provider.GetRequiredService<ICqrs>();
    }
    
    [Fact]
    public async Task ItHandlesCommands()
    {
        var command = new DoIt();

        await sut.Handle(command);

        A.CallTo(() => commandHandler.Handle(command)).MustHaveHappened();
    }

    [Fact]
    public async Task ItHandlesQueries()
    {
        var response = await sut.Handle(new Ping { Message = "Ping" });
        
        response.ShouldBe("Ping pong");
    }

    [Fact]
    public async Task ItThrowsExceptionWhenNoHandlerIsFound()
    {
        await Should.ThrowAsync<NoSuchHandlerException>(() => sut.Handle(new MissingHandler()));
    }
}

public class DoIt : ICommand
{
}


public class MissingHandler : IQuery<string>
{
}

public class Ping : IQuery<string>
{
    public string? Message { get; init; } 
}

public class PingHandler : IQueryHandler<Ping, string>
{
    public Task<string> Handle(Ping subject)
    {
        return Task.FromResult(subject.Message + " pong");
    }
}