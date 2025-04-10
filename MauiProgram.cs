using Microsoft.Extensions.Logging;
using Stargazer.Database;
using Stargazer.Services;
using Stargazer.Views;

namespace Stargazer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IRestService, RestService>();
            builder.Services.AddSingleton<IPlanetsService, PlanetsService>();
            builder.Services.AddSingleton<IStarsService, StarsService>();

            builder.Services.AddSingleton<CelestialDatabase>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<StarPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
