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
            
            DeleteCommand = new DelegateCommand(async () => { await Delete(); });
        }

        private IContactsApi Api { get; }

        private Person person;

        private ApiAction Action { get; set; }

        public DelegateCommand DeleteCommand { get; }

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

        private int internalPhone;

        public int InternalPhone { get => internalPhone; set => SetProperty(ref internalPhone, value); }

        private async void SaveContactDetails()
        {
            person.FirstName = FirstName;
            person.LastName = LastName;
            person.MobileNumber = MobileNumber;
            person.EmailAddress = Email;
            person.InternalPhone = InternalPhone;

            try
            {
                Action = person.Id > 0
                    ? ApiAction.Edited
                    : ApiAction.Added;
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
                {"person",person },
                {"action",Action },
            };

            await NavigationService.GoBackAsync(parameters);            
        }

        private async void Cancel()
        {
            await NavigationService.GoBackAsync();
        }

        private async Task Delete()
        {
            try
            {
                await Api.DeletePersonAsync(person.Id);
                Action = ApiAction.Deleted;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("error", ex.Message, "Ok");
            }

            var parameters = new NavigationParameters
            {
                {"person",person },
                {"action",Action },
            };
            await NavigationService.GoBackAsync(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {

            person = parameters["person"] as Person ?? new Person();
            
            FirstName = person.FirstName;
            LastName = person.LastName;
            MobileNumber = person.MobileNumber;
            Email = person.EmailAddress;
            InternalPhone = person.InternalPhone;

            base.OnNavigatedTo(parameters);
        }

    }
}
