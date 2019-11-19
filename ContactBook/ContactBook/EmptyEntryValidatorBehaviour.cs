using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ContactBook
{
    class EmptyEntryValidatorBehavior : Behavior<ExtendedContactDetailsEntry>
    {

        ExtendedContactDetailsEntry control;
        string _placeholder;

        protected override void OnAttachedTo(ExtendedContactDetailsEntry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            bindable.PropertyChanged += OnPropertyChanged;
            control = bindable;
            _placeholder = bindable.Placeholder;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                ((ExtendedContactDetailsEntry)sender).IsBorderErrorVisible = false;
            }
        }
        protected override void OnDetachingFrom(ExtendedContactDetailsEntry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            bindable.PropertyChanged -= OnPropertyChanged;
        }
        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs ev)
        { 
          if(ev.PropertyName == 
                ExtendedContactDetailsEntry.IsBorderErrorVisibleProperty.PropertyName && control !=null)
            {
                if(control.IsBorderErrorVisible)
                {
                    control.Placeholder = control.ErrorText;
                    control.Text = string.Empty;
                }

                else
                {
                    control.Placeholder = _placeholder;
                }
            }
        }
    }
}
