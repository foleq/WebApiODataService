using developwithpassion.specifications.rhinomocks;
using Machine.Specifications;
using ODataClient.Configurations;
using ODataClient.Helpers;
using ODataClient.Models;
using Rhino.Mocks;
using System.Collections.Generic;
using System.Linq;
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
            _repositoryConfiguration.Stub(x => x.ODataEndpointUrl).Return(_oDataEndpointUrl);
            _repositoryConfiguration.Stub(x => x.EntitySetName).Return(_entitySetName);

            httpHandler = depends.on<IHttpHandler>();
        };

        protected static void SetResultForHttpHanlder(string requestUrl, List<EntityTest> documents, string nextPageUrl = null)
        {
            httpHandler.Stub(x => x.GetAsync<ODataResult<EntityTest>>(requestUrl))
                .Return(Task.FromResult(new ODataResult<EntityTest>
                {
                    Documents = documents,
                    NextPageLink = nextPageUrl,
                }));
        }

        private static IRepositoryConfiguration<EntityTest> _repositoryConfiguration;
        private static readonly string _oDataEndpointUrl = "http://odataendpointurl/";
        private static readonly string _entitySetName = "entitySet";
        protected static readonly string urlForGetDocumentsFromEntitySet = _oDataEndpointUrl + _entitySetName;
        protected static IHttpHandler httpHandler;
    }

    public class when_getting_collection_with_1_page : ODataRepositorySpecs
    {
        Establish context = () =>
        {
            SetResultForHttpHanlder(urlForGetDocumentsFromEntitySet, _collectionFromHttpHandler);
        };

        Because of = () =>
            _collectionResult = sut.GetCollection();

        It should_return_proper_collection = () =>
            _collectionResult.ShouldEqual(_collectionFromHttpHandler);

        private static List<EntityTest> _collectionFromHttpHandler = new List<EntityTest>();
        private static ICollection<EntityTest> _collectionResult;
    }

    public class when_getting_collection_with_3_pages : ODataRepositorySpecs
    {
        Establish context = () =>
        {
            var urlForGetDocumentsFromEntitySetFor2ndPage = "http://url_for_2nd_page";
            var urlForGetDocumentsFromEntitySetFor3rdPage = "http://url_for_3rd_page";
            SetResultForHttpHanlder(urlForGetDocumentsFromEntitySet, new List<EntityTest>() { _entityTest_1 }, urlForGetDocumentsFromEntitySetFor2ndPage);
            SetResultForHttpHanlder(urlForGetDocumentsFromEntitySetFor2ndPage, new List<EntityTest>() { _entityTest_2 }, urlForGetDocumentsFromEntitySetFor3rdPage);
            SetResultForHttpHanlder(urlForGetDocumentsFromEntitySetFor3rdPage, new List<EntityTest>() { _entityTest_3 });
        };

        Because of = () =>
            _collectionResult = sut.GetCollection();

        It should_return_3_documents_in_collection = () =>
            _collectionResult.Count.ShouldEqual(3);

        It should_return_documents_from_each_page_in_proper_order = () =>
        {
            var documents = _collectionResult.ToArray();
            documents[0].ShouldEqual(_entityTest_1);
            documents[1].ShouldEqual(_entityTest_2);
            documents[2].ShouldEqual(_entityTest_3);
        };

        private static readonly EntityTest _entityTest_1 = new EntityTest(), _entityTest_2 = new EntityTest(), _entityTest_3 = new EntityTest();
        private static ICollection<EntityTest> _collectionResult;
    }
}
