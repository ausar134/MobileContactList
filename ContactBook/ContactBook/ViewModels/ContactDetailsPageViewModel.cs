using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ContactBook.Models;
using ContactBook.Views;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using Refit;
using Serilog.Context;

namespace ContactBook.ViewModels
{
    public class ContactDetailsPageViewModel : ViewModelBase
        {
        public ContactDetailsPageViewModel(INavigationService navigationService, IContactsApi api, ILogger<ContactDetailsPageViewModel> logger)
            : base(navigationService, logger, api)
        {
            Title = "Contact Details";
            SaveContactDetailsCommand = new DelegateCommand(SaveContactDetails);
            CancelCommand = new DelegateCommand(Cancel);

            DeleteCommand = new DelegateCommand(async () =>
            {
                using (LogContext.PushProperty("X-Correlation-Id", Guid.NewGuid()))
                {
                    await Delete();
                }
            });
        }

        private Person person;

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value, () =>
            {
                RaisePropertyChanged(nameof(CanDelete));
                RaisePropertyChanged(nameof(IsNotBusy));
            });
        }

        public bool IsNotBusy => !IsBusy;

        public bool CanDelete => person?.Id > 0 && !IsBusy;

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
                return;
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
                IsBusy = true;
                await Task.Delay(5000);
                Logger.LogTrace(LogMessages.DeletingContact, person);
                await Api.DeletePersonAsync(person.Id);
                Action = ApiAction.Deleted;
            }
            catch (ApiException exception) when (exception.StatusCode==HttpStatusCode.NotFound)
            {
                Logger.LogError(exception, LogMessages.TriedToDeleteNonExistingContact, person.Id);
                await App.Current.MainPage.DisplayAlert("error", "No such contact on the server!", "Ok");
                return;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Something went wrong! Reason: {Reason}", exception.Message);
                throw exception;
                return;
            }
            finally
            {
                IsBusy = false;
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
            RaisePropertyChanged(nameof(CanDelete));
            base.OnNavigatedTo(parameters);
        } 
    }
}
