using ContactList.Helpers;
using ContactList.Models;
using ContactList.ViewModels;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ContactList
{
    public interface IEventMap
    {
        void Wire(object viewModel);
        void Unwire(object viewModel);
    }

    public class EventMap<TViewModel, TDelegate> : IEventMap
    {
        public TDelegate Action { get; set; }
        public Action<TViewModel, TDelegate> Arrive { get; set; }
        public Action<TViewModel, TDelegate> Leave { get; set; }

        public EventMap(
                TDelegate action,
                Action<TViewModel, TDelegate> arrive,
                Action<TViewModel, TDelegate> leave)
        {
            Action = action;
            Arrive = arrive;
            Leave = leave;
        }

        public void Wire(object viewModel)
        {
            Arrive((TViewModel)viewModel, Action);
        }

        public void Unwire(object viewModel)
        {
            Leave((TViewModel)viewModel, Action);
        }
    }

    public partial class AppShell : Xamarin.Forms.Shell
    {
        public static IDictionary<Type, IEventMap[]> Maps { get; } = new Dictionary<Type, IEventMap[]>();

        public AppShell()
        {
            InitializeComponent();
            Maps.For<NewItemViewModel>()
            .Do(new EventHandler<Item>((s, args) => Navigation.PopModalAsync()))
            .When((vm, a) => vm.ItemSaved += a, (vm, a) => vm.ItemSaved -= a);
        }
    }
}
