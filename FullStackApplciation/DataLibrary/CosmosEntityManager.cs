using Microsoft.Extensions.Options;
using ModelsLibrary;

namespace DataLibrary
{
    public class CosmosEntityManager
    {
        private readonly IOptions<CosmosDbConfigurationManager> _cdbSettings;

        public CosmosEntityManager(IOptions<CosmosDbConfigurationManager> cdbSettings) 
        {
            _cdbSettings = cdbSettings;
        }

        public CosmosDbConfigurationManager GetConfigurations()
        {
            var cosmosConfig = new CosmosDbConfigurationManager
            {
                CosmosEndpoint = _cdbSettings.Value.CosmosEndpoint,
                CosmosKey = _cdbSettings.Value.CosmosKey
            };
            return cosmosConfig;
        }
    }
}
