namespace af_userapi;

public class BaseFunction
{
    private IMediator mediator;
    private readonly HttpContext httpContext;

    public BaseFunction(IHttpContextAccessor httpContextAccessor)
    {
        httpContext = httpContextAccessor.HttpContext;
    }

    /// <summary>
    /// Gets the Mediator.
    /// </summary>
    protected IMediator Mediator => this.mediator ??= httpContext.RequestServices.GetService<IMediator>();
}
