using Prism;
using Prism.Ioc;
using ContactBook.ViewModels;
using ContactBook.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ContactBook.Models;
using Refit;
using Xamarin.Forms.Internals;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ContactBook
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            
            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void OnStart()
        {
            
            base.OnStart();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(RestService.For<IContactsApi>("https://api.nessos.gr"));
         
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
			containerRegistry.RegisterForNavigation<ContactDetailsPage, ContactDetailsPageViewModel>();
        }
    }
}
