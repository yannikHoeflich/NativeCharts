using Microsoft.Extensions.DependencyInjection;

namespace BlazorCharts;

public static class ServiceExtensions {
    public static void AddBlazorCharts(this IServiceCollection services) {
        services.AddScoped<CanvasService>();
    }
}