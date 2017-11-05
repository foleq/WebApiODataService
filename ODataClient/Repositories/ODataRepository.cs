﻿using ODataClient.Builders;
using ODataClient.Helpers;
using ODataClient.Models;
using System.Collections.Generic;

namespace ODataClient
{
    public interface IODataRepository<TModel>
        where TModel : class
    {
        ICollection<TModel> GetCollection(ODataSearchParameters oDataSearchParameters);
    }

    public class ODataRepository<TModel> : IODataRepository<TModel>
        where TModel : class
    {
        private readonly IHttpHandler _httpHandler;
        private readonly IODataUrlBuilder<TModel> _oDataUrlBuilder;

        public ODataRepository(IHttpHandler httpHandler,
            IODataUrlBuilder<TModel> oDataUrlBuilder)
        {
            _httpHandler = httpHandler;
            _oDataUrlBuilder = oDataUrlBuilder;
        }

        public ICollection<TModel> GetCollection(ODataSearchParameters oDataSearchParameters)
        {
            var url = _oDataUrlBuilder.BuildODataUrl(oDataSearchParameters);
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
