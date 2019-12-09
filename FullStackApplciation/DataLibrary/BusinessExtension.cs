
using DataLayerAbstraction;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DataLibrary
{
    public static class BusinessExtension
    {
        public static IServiceCollection AddDataLibrary (this IServiceCollection services)
        {
           
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddSingleton<ICosmosManager>(InitializeCosmosClientAsync().GetAwaiter().GetResult());
            return services;
        }

        private static async Task<CosmosManager> InitializeCosmosClientAsync()
        {
            string databaseName = "LocalDatabase";
            string containerName = "TestContainer";
            string cosmosEndpoint = "https://localhost:8081";
            string key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            CosmosClientBuilder clientBuilder = new CosmosClientBuilder(cosmosEndpoint, key);
            CosmosClient client = clientBuilder
                                .WithConnectionModeDirect()
                                .Build();
            CosmosManager cosmosDbService = new CosmosManager(client, databaseName, containerName);
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            return cosmosDbService;
        }
    }
}
