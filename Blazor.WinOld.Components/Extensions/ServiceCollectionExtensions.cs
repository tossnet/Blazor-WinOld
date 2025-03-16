using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blazor.WinOld;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWinOldComponents(this IServiceCollection services)
    {
        return services.AddWinOldComponents(ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddWinOldComponents(this IServiceCollection services, ServiceLifetime serviceLifetime)
    {
        services.TryAdd(new ServiceDescriptor(typeof(IDialogService), typeof(DialogService), serviceLifetime));
        return services;
    }
}
