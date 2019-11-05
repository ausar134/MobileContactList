using ContactList.Models;
using ContactList.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ContactList.ViewModels
{
    public class NewItemViewModel:BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command AddItemsCommand { get; set; }



        public NewItemViewModel()
        {
            Title = "Add new contact";
            Items = new ObservableCollection<Item>();
            

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }
        public async Task<bool> AddItemAsync(Item item)
        {
            Items.Add(item);

            return await Task.FromResult(true);
        }

    }
}
