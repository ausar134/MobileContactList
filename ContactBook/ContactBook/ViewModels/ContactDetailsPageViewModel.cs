using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContactBook.Models;
using ContactBook.Views;
using Microsoft.Extensions.Logging;
using Plugin.Media;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Refit;
using Serilog.Context;
using Xamarin.Forms;

namespace ContactBook.ViewModels
{
    public class ContactDetailsPageViewModel : ViewModelBase
    {
        public ContactDetailsPageViewModel(INavigationService navigationService, IContactsApi api,
            ILogger<ContactDetailsPageViewModel> logger, IPageDialogService pageDialogService)
            : base(navigationService, logger, api)
        {
            Title = "Contact Details";

            //errorListBox.DataSource = errors;

            _pageDialogService = pageDialogService;

            TakePhotoCommand = new DelegateCommand(TakePhoto);

            SaveContactDetailsCommand = new DelegateCommand(SaveContactDetails);
            CancelCommand = new DelegateCommand(Cancel);

            DeleteCommand = new DelegateCommand(async () =>
            {


                using (LogContext.PushProperty("X-Correlation-Id", Guid.NewGuid()))
                {
                    await Delete();
                }
            });
            
            OnValidationCommand = new Command((obj) => 
            {
                person.FirstName.NotValidMessageError = "First Name is required";
                //person.FirstName.NotValidMessageError
                //    = "First Name must have more than 2 characters and less than 30";
                person.FirstName.IsNotValid = string.IsNullOrEmpty(person.FirstName.Name);

                person.FirstName.IsNotValid = person.FirstName.Name.Length < 2 || person.FirstName.Name.Length > 30;

                person.LastName.NotValidMessageError = "Last Name is required";
                person.LastName.IsNotValid = (string.IsNullOrEmpty(person.LastName.Name));

                person.MobileNumber.NotValidMessageError = "Mobile Number is required";
                person.MobileNumber.IsNotValid = (string.IsNullOrEmpty(person.MobileNumber.Name));

                person.EmailAddress.NotValidMessageError = "Email Address is required";
                person.EmailAddress.IsNotValid = (string.IsNullOrEmpty(person.EmailAddress.Name));

                person.InternalPhone.NotValidMessageError = "Internal Phone is required";
                person.InternalPhone.IsNotValid = (string.IsNullOrEmpty(person.InternalPhone.Name));
            });
        }

        private Person person;

        private IPageDialogService _pageDialogService;

        private string imagePath; 
        public string ImagePath { 
            get { return imagePath; }
            set { imagePath = value; SetProperty(ref imagePath, value); } }

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

        public DelegateCommand DeleteCommand { get; private set; }

        public DelegateCommand TakePhotoCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveContactDetailsCommand { get; }
   
        public bool ErrorMessageVisiliby { get; set; }
        public ICommand OnValidationCommand { get; set; }

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

        List<string> errors = new List<string>();

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
            catch (Exception e)
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

        async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await _pageDialogService.DisplayAlertAsync("No Camera", ":( No camera available.", "OK");

                return;
            }
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;
            ImagePath = file.Path;
        }

        private async Task Delete()
        {

            #region Delete Confirmation
            var result = await _pageDialogService.DisplayAlertAsync
                ("Alert", "Are you sure you want to delete this contact?", "Delete", "Cancel");

            if (!result)
            {
                return;
            }
            #endregion

            #region Delete procedure
            try
            {
                IsBusy = true;
                await Task.Delay(5000);
                Logger.LogTrace(LogMessages.DeletingContact, person);
                await Api.DeletePersonAsync(person.Id);
                Action = ApiAction.Deleted;
            }
            catch (ApiException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
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
        #endregion

        

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
