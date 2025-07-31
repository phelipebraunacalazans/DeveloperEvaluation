using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Configuration;

public static class WebApiStartup
{
    public static void RegisterWebApiServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddHttpContextAccessor();

        services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(ApplicationLayer).Assembly,
                typeof(Program).Assembly
            );
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped<ICurrentUserAccessor, HttpLoggedUserAccessor>();
    }
}