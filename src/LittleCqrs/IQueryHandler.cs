namespace LittleCqrs;

public interface IQueryHandler<in TQuery, TResponse> where  TQuery : IQuery<TResponse>
{
    Task<TResponse> Handle(TQuery subject);
}