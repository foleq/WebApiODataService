using developwithpassion.specifications.rhinomocks;
using Machine.Specifications;
using ODataClient.Builders;
using ODataClient.Helpers;
using ODataClient.Models;
using ODataClient.Tests.HelpersForTests;
using Rhino.Mocks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataClient.Tests.Repositories
{
    [Subject(typeof(ODataRepository<EntityTest>))]
    public abstract class ODataRepositorySpecs : Observes<IODataRepository<EntityTest>, ODataRepository<EntityTest>>
    {
        Establish context = () =>
        {
            httpHandler = depends.on<IHttpHandler>();

            _oDataUrlBuilder = depends.on<IODataUrlBuilder<EntityTest>>();
            _oDataUrlBuilder.Stub(x => x.BuildODataUrl(oDataSearchParameters))
                .Return(oDataUrlFromBuilder);
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

        private static IODataUrlBuilder<EntityTest> _oDataUrlBuilder;
        protected static IHttpHandler httpHandler;

        protected static readonly ODataSearchParameters oDataSearchParameters = new ODataSearchParameters();
        protected static string oDataUrlFromBuilder = "url from builder";
    }

    public class when_getting_collection_with_1_page : ODataRepositorySpecs
    {
        Establish context = () =>
        {
            SetResultForHttpHanlder(oDataUrlFromBuilder, _collectionFromHttpHandler);
        };

        Because of = () =>
            _collectionResult = sut.GetCollection(oDataSearchParameters);

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
            SetResultForHttpHanlder(oDataUrlFromBuilder, new List<EntityTest>() { _entityTest_1 }, urlForGetDocumentsFromEntitySetFor2ndPage);
            SetResultForHttpHanlder(urlForGetDocumentsFromEntitySetFor2ndPage, new List<EntityTest>() { _entityTest_2 }, urlForGetDocumentsFromEntitySetFor3rdPage);
            SetResultForHttpHanlder(urlForGetDocumentsFromEntitySetFor3rdPage, new List<EntityTest>() { _entityTest_3 });
        };

        Because of = () =>
            _collectionResult = sut.GetCollection(oDataSearchParameters);

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
