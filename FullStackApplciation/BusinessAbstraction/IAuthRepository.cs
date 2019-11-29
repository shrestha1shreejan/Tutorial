using ModelsLibrary.DataModels;
using System.Threading.Tasks;

namespace BusinessAbstraction
{
    public interface IAuthRepository
    {
        Task<Person> Register(Person person, string password);
        Task<Person> Login(string username, string pasword);
        Task<bool> PersonExists(string username);
    }
}
