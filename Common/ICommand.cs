namespace minimal_api.Common;

public interface IGetCommand
{
    public string Path
    {
        get;
    }

    static Task<IResult> Handler<T>(T input) where T : class => throw new NotImplementedException();
}

public interface IPostCommand
{
    public string Path
    {
        get;
    }

    static Task<IResult> Handler<T>(T input) where T : class => throw new NotImplementedException();
}
