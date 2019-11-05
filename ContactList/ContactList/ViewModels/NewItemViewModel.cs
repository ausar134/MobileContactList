using ContactList.Models;
using ContactList.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContactList.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
       
    public event EventHandler<Item> ItemSaved;
        public event EventHandler Cancel;

        private Item item;
        public Item Item { get => item; set => SetProperty(ref item, value); }

        public ICommand SaveItemCommand { get; }
        public ICommand CancelCommand { get; }

        public NewItemViewModel()
        {
            SaveItemCommand = new Command(() =>
            {
                MessagingCenter.Send(this, "AddItem", Item);
                ItemSaved?.Invoke(this, Item);
            });
            CancelCommand = new Command(() =>
            {
                Cancel?.Invoke(this, EventArgs.Empty);
            });
        }
    }

}

