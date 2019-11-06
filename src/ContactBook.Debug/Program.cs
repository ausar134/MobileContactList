using System;
using System.Threading.Tasks;
using ContactBook.Models;
using Refit;

namespace ContactBook.Debug
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var api = RestService.For<IContactsApi>("https://api.nessos.gr/contacts");
            try
            {
                //var person = await api.AddPersonAsync(new Person
                //{
                //    EmailAddress = "akritikos@nessos.gr",
                //    FirstName = "Alexandros",
                //    LastName = "Kritikos",
                //    MobileNumber = "6949975557",
                //    InternalPhone = 109,
                //});
                var persons = await api.GetPeopleAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Hello World!");
        }
    }
}
