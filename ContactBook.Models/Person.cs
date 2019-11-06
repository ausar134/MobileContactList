namespace ContactBook.Models
{
    public class Person
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public int InternalPhone { get; set; }

        public string MobileNumber { get; set; }
    }
}
