using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ContactBook
{
    public class ErrorStateManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        Dictionary<string, ErrorState> states = new Dictionary<string, ErrorState>();

        internal static void Add(object EmailAddress, object message)
        {
            throw new NotImplementedException();
        }
    }
}
