using ModelsLibrary.DataModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DataLayerAbstraction
{
    public interface ICosmosManager
    {
        Task<IEnumerable<Person>> GetUsers();
        Task<Person> GetPersonById(string id);
        Task<HttpStatusCode> AddPersonData(Person person);
        Task<HttpStatusCode> UpdatePersonsData(string id, Person person);
        Task<bool> PersonExists(string username);
        Task<Person> GetPersonByUsername(string username);
    }
}
