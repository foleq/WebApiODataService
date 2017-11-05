using ODataClient.Configurations;
using ODataClient.Models;
using System.Collections.Generic;
using System.Text;

namespace ODataClient.Builders
{
    public interface IODataUrlBuilder<TModel>
        where TModel : class
    {
        string BuildODataUrl(ODataSearchParameters oDataSearchParameters);
    }

    public class ODataUrlBuilder<TModel> : IODataUrlBuilder<TModel>
        where TModel : class
    {
        private readonly IRepositoryConfiguration<TModel> _repositoryConfiguration;
        private readonly string _odataEntitySetUrl;

        public ODataUrlBuilder(IRepositoryConfiguration<TModel> repositoryConfiguration)
        {
            _repositoryConfiguration = repositoryConfiguration;
            _odataEntitySetUrl = _repositoryConfiguration.ODataEndpointUrl + _repositoryConfiguration.EntitySetName;
        }

        public string BuildODataUrl(ODataSearchParameters oDataSearchParameters)
        {
            var url = _odataEntitySetUrl;
            var parameters = new List<string>();

            if(oDataSearchParameters.Filter != null)
            {
                parameters.Add($"$filter={oDataSearchParameters.Filter.Query}");
            }
            if(oDataSearchParameters.Top.HasValue)
            {
                parameters.Add($"$top={oDataSearchParameters.Top.Value}");
            }

            if(parameters.Count > 0)
            {
                var parametersQuery = string.Join("&", parameters);
                url += $"?{parametersQuery}";
            }
            return url.ToString();
        }
    }
}
