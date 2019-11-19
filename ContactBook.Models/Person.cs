using System.ComponentModel;

namespace ContactBook.Models
{
    public class Person : INotifyPropertyChanged
    {
        public int Id { get; set; } 

        public Field FirstName { get; set; } = new Field();

        public Field LastName { get; set; } = new Field();

        public Field EmailAddress { get; set; } = new Field();

        public Field InternalPhone { get; set; } = new Field();

        public Field MobileNumber { get; set; } = new Field();

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
