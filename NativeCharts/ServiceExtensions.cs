using Microsoft.Extensions.DependencyInjection;

namespace NativeCharts;

public static class ServiceExtensions {
    public static void AddNativeCharts(this IServiceCollection services) {
        services.AddScoped<CanvasService>();
    }
}