using System;
using System.Collections.Generic;
using System.Text;
using ContactBook.Models;
using ContactBook.Views;
using Prism.Commands;
using Prism.Navigation;


namespace ContactBook.ViewModels
{
    public class ContactDetailsPageViewModel : ViewModelBase
    {
        public ContactDetailsPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Contact Details";

            SaveContactDetailsCommand = new DelegateCommand(SaveContactDetails);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private Person person;

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

            var parameters = new NavigationParameters
            {
                {"person",person }
            };
            await NavigationService.GoBackAsync();
        }

        private async void Cancel()
        {
            await NavigationService.GoBackAsync();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);


            //Explain these...
            var p = parameters["person"] as Person;
            person = p ?? new Person();

            FirstName = person.FirstName;
            LastName = person.LastName;
            MobileNumber = person.MobileNumber;
            Email = person.EmailAddress;
        }

    }
}
