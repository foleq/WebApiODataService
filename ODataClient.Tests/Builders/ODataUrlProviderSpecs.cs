using developwithpassion.specifications.rhinomocks;
using Machine.Specifications;
using ODataClient.Builders;
using ODataClient.Configurations;
using ODataClient.Models;
using ODataClient.Tests.HelpersForTests;
using Rhino.Mocks;

namespace ODataClient.Tests.Providers
{
    [Subject(typeof(ODataUrlBuilder<EntityTest>))]
    abstract class ODataUrlBuilderSpecs : Observes<IODataUrlBuilder<EntityTest>, ODataUrlBuilder<EntityTest>>
    {
        Establish context = () =>
        {
            _repositoryConfiguration = depends.on<IRepositoryConfiguration<EntityTest>>();
            _repositoryConfiguration.Stub(x => x.ODataEndpointUrl).Return(_oDataEndpointUrl);
            _repositoryConfiguration.Stub(x => x.EntitySetName).Return(_entitySetName);
        };

        private static IRepositoryConfiguration<EntityTest> _repositoryConfiguration;
        private static readonly string _oDataEndpointUrl = "http://odataendpointurl/";
        private static readonly string _entitySetName = "entitySet";
        protected static readonly string urlForGetDocumentsFromEntitySet = _oDataEndpointUrl + _entitySetName;
    }

    abstract class when_building_url : ODataUrlBuilderSpecs
    {
        Because of = () =>
        {
            urlResult = sut.BuildODataUrl(oDataSearchParameters);
        };

        protected static ODataSearchParameters oDataSearchParameters;
        protected static string urlResult;
    }

    class when_building_url_from_empty_search_parameters : when_building_url
    {
        Establish context = () =>
        {
            oDataSearchParameters = new ODataSearchParameters();
        };

        It should_return_proper_url = () =>
            urlResult.ShouldEqual(urlForGetDocumentsFromEntitySet);
    }

    class when_building_url_with_filter : when_building_url
    {
        Establish context = () =>
        {
            oDataSearchParameters = new ODataSearchParameters()
            {
                Filter = new ODataFilter()
                {
                    Query = "filter condition"
                }
            };
        };

        It should_return_proper_url = () =>
            urlResult.ShouldEqual(urlForGetDocumentsFromEntitySet + "?$filter=filter condition");
    }

    class when_building_url_with_set_top : when_building_url
    {
        Establish context = () =>
        {
            oDataSearchParameters = new ODataSearchParameters()
            {
                Top = 99
            };
        };

        It should_return_proper_url = () =>
            urlResult.ShouldEqual(urlForGetDocumentsFromEntitySet + "?$top=99");
    }

    class when_building_url_with_filter_and_set_top : when_building_url
    {
        Establish context = () =>
        {
            oDataSearchParameters = new ODataSearchParameters()
            {
                Filter = new ODataFilter()
                {
                    Query = "some filter condition"
                },
                Top = 55
            };
        };

        It should_return_proper_url = () =>
            urlResult.ShouldEqual(urlForGetDocumentsFromEntitySet + "?$filter=some filter condition&$top=55");
    }
}
