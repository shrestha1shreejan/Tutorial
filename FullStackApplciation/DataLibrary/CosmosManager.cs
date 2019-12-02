using DataLayerAbstraction;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using ModelsLibrary;
using ModelsLibrary.DataModels;
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
        public async Task<IEnumerable<Person>> GetUsers()
        {           
            //var query = "SELECT * FROM c";
            //var iterator = _container.GetItemQueryIterator<Person>(query);
            //var data = await iterator.ReadNextAsync();
            //return data;

            var query = this._container.GetItemQueryIterator<Person>(new QueryDefinition("SELECT * FROM c"));
            List<Person> results = new List<Person>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();                
                results.AddRange(response.ToList());
            }

            return results;
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
            var persons = await GetUsers();
            foreach (var person in persons)
            {
                if (person.Username.Equals(username))
                {
                    return true;
                }
            }
            return false;
        }

    }
} 
