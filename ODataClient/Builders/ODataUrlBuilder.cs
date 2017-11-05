using ODataClient.Configurations;
using ODataClient.Models;
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
            var url = new StringBuilder(_odataEntitySetUrl);
            url.Append("?");
            if(oDataSearchParameters.Filter != null)
            {
                url.Append("$filter=");
                url.Append(oDataSearchParameters.Filter.Query);
            }
            return url.ToString();
        }
    }
}
