using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Extensions.Logging;
using Tumirai.Pages;
using Tumirai.Services;
using Tumirai.ViewModels;

namespace Tumirai
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
                    fonts.AddFont("fontello.ttf", "Icons");
                    fonts.AddFont("Epilogue-Medium-500.ttf", "Epilogue");
                })
                .UseMauiMaps();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Initialize Firebase and add it to DI
            Initialize();

            // Register services in DI container
            builder.Services.AddSingleton<SignInView>();
            builder.Services.AddSingleton<SignUpView>();
            builder.Services.AddSingleton<SignInViewModel>(); // This will inject FirebaseAuthClient into the SignInViewModel
            builder.Services.AddSingleton<SignUpViewModel>();
            builder.Services.AddSingleton<IFirebaseService, FirebaseService>();
            builder.Services.AddTransient<HomeViewModel>();


            // Register the FirebaseAuthClient as a singleton
            builder.Services.AddSingleton(AuthClient);

            return builder.Build();
        }

        public static FirebaseAuthClient AuthClient { get; private set; }

        // Initialize FirebaseAuthClient
        public static void Initialize()
        {
            var config = new FirebaseAuthConfig
            {
                

                ApiKey = "AIzaSyDUBca_oIartn0YaiUuetmWDcj1z-gMa2s",
                AuthDomain = "tumirei.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            };

            // Instantiate the FirebaseAuthClient directly
            AuthClient = new FirebaseAuthClient(config);
        }
    }
}
