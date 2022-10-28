using System.Linq.Expressions;
using System.Reflection;
using minimal_api.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


Assembly.GetExecutingAssembly().GetTypesAssignableFrom<IGetCommand>().ForEach((t) =>
{
    var command = (IGetCommand)Activator.CreateInstance(t);
    var handler = command.GetType().GetMethod("Handler");
    var handlerDelegate = AssemblyExtension.CreateDelegate(handler);
    app.MapGet(command.Path, handlerDelegate);
});

Assembly.GetExecutingAssembly().GetTypesAssignableFrom<IPostCommand>().ForEach((t) =>
{
    var command = (IPostCommand)Activator.CreateInstance(t);
    var path = command.Path;
    var handler = command.GetType().GetMethod("Handler");
    var handlerDelegate = AssemblyExtension.CreateDelegate(handler);
    app.MapPost(path, handlerDelegate); 
});

app.Run();

public static class AssemblyExtension
{
    public static List<Type> GetTypesAssignableFrom<T>(this Assembly assembly)
    {
        return assembly.GetTypesAssignableFrom(typeof(T));
    }
    public static List<Type> GetTypesAssignableFrom(this Assembly assembly, Type compareType)
    {
        List<Type> ret = new List<Type>();
        foreach (var type in assembly.DefinedTypes)
        {
            if (compareType.IsAssignableFrom(type) && compareType != type)
            {
                ret.Add(type);
            }
        }
        return ret;
    }

    public static Delegate CreateDelegate(MethodInfo method)
    {
        if (method == null)
        {
            throw new ArgumentNullException("method");
        }

        if (!method.IsStatic)
        {
            throw new ArgumentException("The provided method must be static.", "method");
        }

        if (method.IsGenericMethod)
        {
            throw new ArgumentException("The provided method must not be generic.", "method");
        }

        return method.CreateDelegate(Expression.GetDelegateType(
            (from parameter in method.GetParameters() select parameter.ParameterType)
            .Concat(new[] { method.ReturnType })
            .ToArray()));
    }
}


var app = GtmWebApiApplication.Create(args);

await app.Run();