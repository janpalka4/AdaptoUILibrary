using Microsoft.Extensions.DependencyInjection;

namespace AdaptoUILibrary.Classes
{
    public static class ServiceCollectionExtension
    {
        public static void AddAdaptoUILibrary(this IServiceCollection services)
        {
            services.AddScoped<ITabsEventManager, TabsEventManager>();
            services.AddScoped<IElementMeasurementService, ElementMeasurementService>();
            services.AddSingleton(new DragTemp());
            services.AddSingleton(new DockContentHelper());
        }
    }
}
