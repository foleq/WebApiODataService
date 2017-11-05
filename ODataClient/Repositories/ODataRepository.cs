using ODataClient.Configurations;
using ODataClient.Helpers;
using ODataClient.Models;
using System.Collections.Generic;

namespace ODataClient
{
    public interface IODataRepository<TModel>
        where TModel : class
    {
        ICollection<TModel> GetCollection();
    }

    public class ODataRepository<TModel> : IODataRepository<TModel>
        where TModel : class
    {
        private readonly IRepositoryConfiguration<TModel> _repositoryConfiguration;
        private readonly IHttpHandler _httpHandler;
        private readonly string _odataEntitySetUrl;

        public ODataRepository(IRepositoryConfiguration<TModel> repositoryConfiguration,
            IHttpHandler httpHandler)
        {
            _repositoryConfiguration = repositoryConfiguration;
            _httpHandler = httpHandler;
            _odataEntitySetUrl = _repositoryConfiguration.ODataEndpointUrl + _repositoryConfiguration.EntitySetName;
        }

        public ICollection<TModel> GetCollection()
        {
            var url = _odataEntitySetUrl;
            var documents = new List<TModel>();

            do
            {
                var batchResult = GetDocumentsBatch(url);
                documents.AddRange(batchResult.Documents);
                url = batchResult.NextPageLink;
            } while (!string.IsNullOrWhiteSpace(url));

            return documents;
        }

        private ODataResult<TModel> GetDocumentsBatch(string url)
        {
            var task = _httpHandler.GetAsync<ODataResult<TModel>>(url);
            task.Wait();
            return task.Result;
        }
    }
}
