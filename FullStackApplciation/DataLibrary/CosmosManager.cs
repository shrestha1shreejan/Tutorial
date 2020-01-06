using DataLayerAbstraction;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using ModelsLibrary;
using ModelsLibrary.DataModels;
using ModelsLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class CosmosManager : ICosmosManager
    {
        // private readonly IOptions<CosmosDbConfigurationManager> _cdbSettings;
        // private readonly CosmosClient _client;
        private Container _container;

        //public CosmosManager(IOptions<CosmosDbConfigurationManager> cdbSettings)
        //{
        //    _cdbSettings = cdbSettings;
        //    _client = new CosmosClient(_cdbSettings.Value.CosmosEndpoint, _cdbSettings.Value.CosmosKey);
        //}
        public CosmosManager(CosmosClient dbClient, string databaseName, string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }




        /// <summary>
        /// Get  All User Data
        /// </summary>
        /// <returns></returns>
        public async Task<PagedList<Person>> GetUsers(UserParams userParams)
        {           
            //var query = "SELECT * FROM c";
            //var iterator = _container.GetItemQueryIterator<Person>(query);
            //var data = await iterator.ReadNextAsync();
            //return data;

            var query = this._container.GetItemQueryIterator<Person>(new QueryDefinition($"SELECT * FROM c WHERE c.id != '{userParams.UserId}' AND c.Gender = '{userParams.Gender}' "));
            List<Person> results = new List<Person>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();                
                results.AddRange(response.OrderByDescending(u => u.LastActive).ToList());
            }
            var x = results;
            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

                x = results.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob).ToList();               
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        x = x.OrderByDescending(u => u.Created).ToList();
                        break;
                    default:
                        x = x.OrderByDescending(u => u.LastActive).ToList();
                        break;
                }
            }
            return PagedList<Person>.Create(x.AsQueryable(), userParams.PageNumber, userParams.pageSize);            
        }

        /// <summary>
        /// Get particular user data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public async Task<Person> GetPersonById(string id)
        {
            // TODO: handle the case where user is not found
            try
            {
                ItemResponse<Person> response = await _container.ReadItemAsync<Person>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (Exception)
            {

                return null;
            }
           
        }


        /// <summary>
        /// Get  All User Data
        /// </summary>
        /// <returns></returns>
        public async Task<Person> GetPersonByUsername(string username)
        {
            var query = $"SELECT * FROM c  WHERE c.Username = '{username}' ";            
            var iterator = _container.GetItemQueryIterator<Person>(query);
            var data = await iterator.ReadNextAsync();
           
            foreach (Person person in data)
            {
                if (person != null)
                {
                    return person;
                }
            }

            return null;
        }


        /// <summary>
        /// Add data persons data to database
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> AddPersonData(Person person)
        {            
            var response = await _container.
                CreateItemAsync<Person>(person, new PartitionKey(person.Id.ToString()));
            return response.StatusCode;
        }
    
      
        /// <summary>
        /// Update persons data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> UpdatePersonsData(string id, Person person)
        {
            var response = await _container.UpsertItemAsync<Person>(person, new PartitionKey(id));
            return response.StatusCode;
        }

        /// <summary>
        /// check if the person username already exists
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> PersonExists(string username)
        {
            
            var query = this._container.GetItemQueryIterator<Person>(new QueryDefinition("SELECT * FROM c"));
            List<Person> results = new List<Person>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            // var persons = await GetUsers(userParams);
            foreach (var person in results)
            {
                if (person.Username.Equals(username))
                {
                    return true;
                }
            }
            return false;
        }


        public async Task<Photo> GetPhotoById(string userId, int photoId)
        {            
            var query = $"SELECT c.Photos FROM c WHERE c.id =  '{userId}' ";
            var iterator = _container.GetItemQueryIterator<Photo>(query);
            List<Photo> photos = new List<Photo>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                photos.AddRange(response.ToList());
            }

            var p = photos.FirstOrDefault(x => x.Id == photoId);
            return p;
        }

    }
} 
