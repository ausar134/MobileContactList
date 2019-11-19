using System.ComponentModel;

namespace ContactBook.Models
{
    public class Person : INotifyPropertyChanged
    {
        public int Id { get; set; } 

        public string FirstName { get; set; } 

        public string LastName { get; set; } 

        public string EmailAddress { get; set; } 

        public int InternalPhone { get; set; } 

        public string MobileNumber { get; set; } 

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Field : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public bool IsNotValid { get; set; }
        public string NotValidMessageError { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
