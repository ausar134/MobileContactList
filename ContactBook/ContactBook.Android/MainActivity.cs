using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;
using Serilog;
using Serilog.Core;
using System;

namespace ContactBook.Droid
{
    [Activity(Label = "ContactBook", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch();
        protected override void OnCreate(Bundle bundle)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .WriteTo.AndroidLog()
                .WriteTo.Seq(
                apiKey: "GquTIyDKLV0vI269oyUb",
                serverUrl: "https://orion.nessos.gr",
                controlLevelSwitch: levelSwitch)
                .CreateLogger();

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var exception = args.ExceptionObject as Exception;
                Log.Fatal(exception, "Unhandled exception: {Message}", exception?.Message ?? string.Empty);
            };

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

