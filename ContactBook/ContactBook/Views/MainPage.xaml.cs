using ContactBook.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContactBook.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void SelectedItem(Object sender, ItemTappedEventArgs e)
        {
            var mydetails = e.Item as Person;
            await Navigation.PushAsync(new ContactDetailsPage(mydetails.FirstName, mydetails.LastName, mydetails.MobileNumber, mydetails.EmailAddress));

        }
    }
}
