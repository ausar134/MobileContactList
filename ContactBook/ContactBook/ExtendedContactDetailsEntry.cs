using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ContactBook
{
   public class ExtendedContactDetailsEntry : Entry
    {
        public static readonly BindableProperty IsBorderErrorVisibleProperty
            = BindableProperty.Create
            (nameof(IsBorderErrorVisible), typeof(bool), 
            typeof(ExtendedContactDetailsEntry), false, BindingMode.TwoWay);

        public static readonly BindableProperty ErrorTextProperty 
            = BindableProperty.Create
            (nameof(ErrorText), 
                typeof(string), typeof(ExtendedContactDetailsEntry), string.Empty);


        public bool IsBorderErrorVisible
        {
            get { return (bool)GetValue(IsBorderErrorVisibleProperty); }
            set
            {
                SetValue(IsBorderErrorVisibleProperty, value);
            }
        }
        public string ErrorText
        {
            get { return (string)GetValue(ErrorTextProperty); }
            set
            {
                SetValue(ErrorTextProperty, value);
            }
        }
    }
}
