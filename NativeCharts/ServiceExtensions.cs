using Microsoft.Extensions.DependencyInjection;

namespace NativeCharts;

public static class ServiceExtensions {
    public static void AddBlazorCharts(this IServiceCollection services) {
        services.AddScoped<CanvasService>();
    }
}