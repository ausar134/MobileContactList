using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using System;
using System.Reflection;
using Xamarin.Essentials;

namespace ContactBook.Droid
{
    [Activity(Label = "ContactBook", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch();
        protected override void OnCreate(Bundle bundle)
        {
            var deviceId = Android.Provider.Settings.Secure.GetString(
                Application.Context.ContentResolver,
                Android.Provider.Settings.Secure.AndroidId);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("DeviceId", deviceId)
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
            
            base.OnCreate(bundle);
            Platform.Init(this, bundle);

            LogContext.PushProperty("Manufacturer", DeviceInfo.Manufacturer);
            LogContext.PushProperty("Model", DeviceInfo.Model);
            LogContext.PushProperty("OS", DeviceInfo.Platform);
            LogContext.PushProperty("Version", DeviceInfo.Version);
            LogContext.PushProperty("Application", Assembly.GetExecutingAssembly().GetName().Name);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

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

