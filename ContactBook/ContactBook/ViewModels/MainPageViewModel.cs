using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ContactBook.Models;
using ContactBook.Views;
using MvvmHelpers;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace ContactBook.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IContactsApi Api { get; }
        private bool _hasInitialized;
        
        public MainPageViewModel(INavigationService navigationService, IContactsApi api)
            : base(navigationService)
        {
            Title = "Nessos Contacts";

            Api = api;

            //Command structure without parameters
            NavigateToContactDetailsCommand = new DelegateCommand(NavigateToContactDetailsPage);

            //Command structure that takes parameters
            TappedItemCommand = new DelegateCommand<Person>(async (p) => await TappedItem(p));

            RefreshCommand = new DelegateCommand(async () =>
             {
                 await Refresh();
             });
        }

        public bool IsSelected => SelectedItem != null;
        private Person _selectedItem;
        public Person SelectedItem {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value, () => RaisePropertyChanged(nameof(IsSelected)));
        }
        
        public DelegateCommand<Person> TappedItemCommand { get; }

        public DelegateCommand RefreshCommand { get; }

        public DelegateCommand NavigateToContactDetailsCommand { get; }

        public ObservableRangeCollection<Person> People { get; } = new ObservableRangeCollection<Person>() { };

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            #region Populates contact list at first launch
            if (!_hasInitialized)
            { 
                RefreshCommand.Execute();
                _hasInitialized = true;
            }
            #endregion

            var action = parameters["action"] as ApiAction?;
            var person = parameters["person"] as Person;
            if (person == null)
                return;

            var existingPerson = People.SingleOrDefault(x => x.Id == person.Id);
            if (existingPerson != null)
            {
                People.Remove(existingPerson);
            }
            if (action != ApiAction.Deleted) 
            {
                People.Add(person);
            }
        }

        private async void NavigateToContactDetailsPage()
        {
            await NavigationService.NavigateAsync("ContactDetailsPage");
        }

        public async Task Refresh()
        {
            try
            {
                var contacts = await Api.GetPeopleAsync();
                People.ReplaceRange(contacts);
            }
            catch (Exception exception)
            {
                await App.Current.MainPage.DisplayAlert("error", exception.Message, "Ok");
            }
        }

        private async Task TappedItem(Person p)
        {
            var parameters = new NavigationParameters
            {
                {"person",p }
            };
            await NavigationService.NavigateAsync("ContactDetailsPage", parameters);
        }
        
    }
}
