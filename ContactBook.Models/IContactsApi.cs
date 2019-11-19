using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Models
{
    public interface IContactsApi
    {
        [Get("/api/contact")]
        Task<List<Person>> GetPeopleAsync();

        [Get("/api/contact/{id}")]
        Task<Person> GetPersonAsync(long id);

        [Post("/api/contact")]
        Task<Person> AddPersonAsync(Person p);

        [Put("/api/contact/{id}")]
        Task<Person> UpdatePersonAsync(long id, Person p);
        
        [Delete("/api/contact/{id}")]
        Task DeletePersonAsync(long id);
        Task<Person> UpdatePersonAsync(Field id, Person person);
    }
}
