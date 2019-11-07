using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ContactBook.Models;
using ContactBook.Views;
using Prism.Commands;
using Prism.Navigation;


namespace ContactBook.ViewModels
{
    public class ContactDetailsPageViewModel : ViewModelBase
    {
        public ContactDetailsPageViewModel(INavigationService navigationService, IContactsApi api)
            : base(navigationService)
        {
            Title = "Contact Details";
            Api = api;
            SaveContactDetailsCommand = new DelegateCommand(SaveContactDetails);
            CancelCommand = new DelegateCommand(Cancel);
            
            DeleteCommand = new DelegateCommand<Person>(async (p) => { await Delete(p); });
        }

        private IContactsApi Api { get; }

        private Person person;

        public DelegateCommand<Person> DeleteCommand { get; }

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveContactDetailsCommand { get; }

        private string firstName;

        public string FirstName { get => firstName; set => SetProperty(ref firstName, value); }

        private string lastName;
        public string LastName { get => lastName; set => SetProperty(ref lastName, value); }

        private string mobileNumber;
        public string MobileNumber { get => mobileNumber; set => SetProperty(ref mobileNumber, value); }

        private string email;

        public string Email { get => email; set => SetProperty(ref email, value); }

        private async void SaveContactDetails()
        {
            person.FirstName = FirstName;
            person.LastName = LastName;
            person.MobileNumber = MobileNumber;
            person.EmailAddress = Email;

            try
            {
                person = await (person.Id > 0
                    ? Api.UpdatePersonAsync(person.Id, person)
                    : Api.AddPersonAsync(person));
            }
            catch(Exception e)
            {
                await App.Current.MainPage.DisplayAlert("error", e.Message, "Ok");
            }
           
            var parameters = new NavigationParameters
            {
                {"person",person }
            };

            await NavigationService.GoBackAsync(parameters);            
        }

        private async void Cancel()
        {
            await NavigationService.GoBackAsync();
        }

        private async Task Delete(Person p)
        {
            try
            {
                await Api.DeletePersonAsync(p.Id);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("error", ex.Message, "Ok");
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            
            var person = parameters["person"] as Person ?? new Person();
            
            FirstName = person.FirstName;
            LastName = person.LastName;
            MobileNumber = person.MobileNumber;
            Email = person.EmailAddress;

            base.OnNavigatedTo(parameters);
        }

    }
}
