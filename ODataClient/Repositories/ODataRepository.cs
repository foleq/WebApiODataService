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

        public ODataRepository(IRepositoryConfiguration<TModel> repositoryConfiguration,
            IHttpHandler httpHandler)
        {
            _repositoryConfiguration = repositoryConfiguration;
            _httpHandler = httpHandler;
        }

        public ICollection<TModel> GetCollection()
        {
            var url = _repositoryConfiguration.ODataEndpointUrl + _repositoryConfiguration.EntitySetName;
            var odataResult = GetDocumentsBatch(url);
            return odataResult.Documents;
        }

        private ODataResult<TModel> GetDocumentsBatch(string url)
        {
            var task = _httpHandler.GetAsync<ODataResult<TModel>>(url);
            task.Wait();
            return task.Result;
        }
    }
}
