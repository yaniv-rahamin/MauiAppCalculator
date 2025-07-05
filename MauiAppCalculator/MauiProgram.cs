
using MauiAppCalculator.Models;
using MauiAppCalculator.ViewModels;
using MauiAppCalculator.Views;
using Microsoft.Extensions.Logging;

namespace MauiAppCalculator
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.RegisterView().Registermodel().RegisterViewModel();
            return builder.Build();
        }
        public static MauiAppBuilder RegisterView(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<calculatorPage>();
            return builder;
        }
        public static MauiAppBuilder RegisterViewModel(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<calculatorViewModel>();
            builder.Services.AddSingleton<ViewModelBase>();
            return builder;
        }
        
        public static MauiAppBuilder Registermodel(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<CalculatorModel>();
            return builder;
        }
    }
}
 
