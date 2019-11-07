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
            var api = RestService.For<IContactsApi>("https://api.nessos.gr");
            try
            {
                
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
