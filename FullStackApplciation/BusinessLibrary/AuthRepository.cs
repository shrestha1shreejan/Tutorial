using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BusinessAbstraction;
using DataLibrary;
using ModelsLibrary.DataModels;

namespace BusinessLibrary
{
    public class AuthRepository : IAuthRepository
    {
        private readonly CosmosManager _context;

        public AuthRepository(CosmosManager context )
        {
            _context = context;
        }


        #region Implemenatation

        /// <summary>
        /// Checks if the user exists
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> PersonExists(string username)
        {
            if (await _context.PersonExists(username))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Login the user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Person> Login(string username, string password)
        {
            var person = await _context.GetPersonByUsername(username);
            var isAuthenticated = VerifyPassword(password, person.PasswordHash, person.PasswordSalt);

            if (!isAuthenticated)
            {
                return null;
            }

            return person;
        }      


        /// <summary>
        /// Register the person 
        /// </summary>
        /// <param name="person"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Person> Register(Person person, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            person.PasswordHash = passwordHash;
            person.PasswordSalt = passwordSalt;

            var statusCode = await _context.AddPersonData(person);

            if (statusCode == HttpStatusCode.Created)
            {
                return person;
            }

            return null;
            
        }

        #endregion



        #region Private Methods

        private void CreatePasswordHash(string password , out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return passwordHash.SequenceEqual(computedHash);
            }
        }
        #endregion

    }
}
