using developwithpassion.specifications.rhinomocks;
using Machine.Specifications;
using ODataClient.Configurations;
using ODataClient.Helpers;
using ODataClient.Models;
using Rhino.Mocks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ODataClient.Tests.Repositories
{
    public class EntityTest
    {
        string Id { get; set; }
        string Name { get; set; }
    }

    [Subject(typeof(ODataRepository<EntityTest>))]
    public abstract class ODataRepositorySpecs : Observes<IODataRepository<EntityTest>, ODataRepository<EntityTest>>
    {
        Establish context = () =>
        {
            _repositoryConfiguration = depends.on<IRepositoryConfiguration<EntityTest>>();
            _repositoryConfiguration.Stub(x => x.ODataEndpointUrl).Return(oDataEndpointUrl);
            _repositoryConfiguration.Stub(x => x.EntitySetName).Return(entitySetName);

            httpHandler = depends.on<IHttpHandler>();
        };

        private static IRepositoryConfiguration<EntityTest> _repositoryConfiguration;
        protected static readonly string oDataEndpointUrl = "http://odataendpointurl/";
        protected static readonly string entitySetName = "entitySet";
        protected static IHttpHandler httpHandler;
    }

    public class when_getting_collection : ODataRepositorySpecs
    {
        Establish context = () =>
        {
            var urlForGetDocumentsFromEntitySet = oDataEndpointUrl + entitySetName;
            httpHandler.Stub(x => x.GetAsync<ODataResult<EntityTest>>(urlForGetDocumentsFromEntitySet))
                .Return(Task.FromResult(new ODataResult<EntityTest>
                {
                    Documents = _collectionFromHttpHandler,
                }));
        };

        Because of = () =>
        {
            _collectionResult = sut.GetCollection();
        };

        It should_return_proper_collection = () =>
        {
            _collectionResult.ShouldEqual(_collectionFromHttpHandler);
        };

        private static List<EntityTest> _collectionFromHttpHandler = new List<EntityTest>();
        private static ICollection<EntityTest> _collectionResult;

        public It Should_return_proper_collection { get => should_return_proper_collection; set => should_return_proper_collection = value; }
    }
}
