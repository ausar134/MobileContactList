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
        private readonly IContactsApi _api;
        private bool _hasInitialized;
        
        public MainPageViewModel(INavigationService navigationService, IContactsApi api)
            : base(navigationService)
        {
            Title = "Nessos Contacts";

            _api = api;

            
            NavigateToContactDetailsCommand = new DelegateCommand(NavigateToContactDetailsPage);

            GetSelectedItemCommand = new DelegateCommand(GetSelectedItem); 

            RefreshCommand = new DelegateCommand(async () =>
             {
                 await Refresh();
             });
        }


        private bool _isSelected = false;
        public bool IsSelected => _isSelected;

        private Person _selectedItem;
        public Person SelectedItem {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value, () => RaisePropertyChanged(nameof(IsSelected)));
        }

        public DelegateCommand GetSelectedItemCommand { get; }
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

            #region Handles contact from add/edit operations
            var person = parameters["person"] as Person;
            if(person == null)
            {
                return;
            }

            var existingPerson = People.SingleOrDefault(x => x.Id == person.Id);
            if (existingPerson == null)
            {
                People.Add(person);
            }
            else
            {
                People.Remove(existingPerson);
                People.Add(person);
            }
            #endregion
        }

        public async void NavigateToContactDetailsPage()
        {
            await NavigationService.NavigateAsync("ContactDetailsPage");
        }

        public async Task Refresh()
        {
            try
            {
                People.ReplaceRange(await _api.GetPeopleAsync());
            }
            catch (Exception exception)
            {
                await App.Current.MainPage.DisplayAlert("error", exception.Message, "Ok");
            }
        }

        public async void GetSelectedItem()
        {
            if (_isSelected == true)
            {
                
                await NavigationService.NavigateAsync("ContactDetailsPage");
            }
        }
        
    }
}
