using ContactBook.Models;
using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using Prism.Navigation;

namespace ContactBook.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        protected ILogger Logger { get; }

        protected IContactsApi Api { get; }

        public ViewModelBase(INavigationService navigationService, ILogger logger, IContactsApi api)
        {
            NavigationService = navigationService;
            Logger = logger;
            Api = api;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }
        
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
