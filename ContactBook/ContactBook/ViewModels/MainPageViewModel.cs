using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ContactBook.Models;
using ContactBook.Views;
using MvvmHelpers;
using Prism.Commands;
using Prism.Navigation;

namespace ContactBook.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IContactsApi _api;
        private readonly DelegateCommand _refreshCommand;
        public MainPageViewModel(INavigationService navigationService, IContactsApi api)
            : base(navigationService)
        {
            Title = "Nessos Contacts";

            _api = api;

            NavigateToContactDetailsCommand = new DelegateCommand(NavigateToContactDetailsPage);
            _refreshCommand = new DelegateCommand(async () =>
             {
                 try
                 {
                     People.ReplaceRange(await api.GetPeopleAsync());
                 }
                 catch(Exception exception)
                 {
                     await App.Current.MainPage.DisplayAlert("error", exception.Message,"Ok");
                 }
             });
        }

        public DelegateCommand NavigateToContactDetailsCommand { get; }

        public ObservableRangeCollection<Person> People { get; } = new ObservableRangeCollection<Person>() { };

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            _refreshCommand.Execute();
        }

        public async void NavigateToContactDetailsPage()
        {
            await NavigationService.NavigateAsync("ContactDetailsPage");
        }


        

       

        

        

    }
}
