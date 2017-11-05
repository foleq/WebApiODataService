using Machine.Specifications;
using ODataClient.Helpers;
using ODataClient.Entities;
using ODataClient.Builders;
using ODataClient.Models;
using ODataClient.Configurations;

namespace ODataClient.Tests
{
    class TestsOnExistingODataServiceSpecs
    {
        It should_work_with_odatarepository = () =>
        {
            var repo = new ODataRepository<Person>(new HttpHandler(),
                new ODataUrlBuilder<Person>(new PersonRepositoryConfiguration()));
            var result = repo.GetCollection(new ODataSearchParameters
            {
                Filter = new ODataFilter()
                {
                    Query = "Trips/any(x: x/ID eq '0003')"
                }
            });
        };

        It should_test = () =>
        {
            new TestRepositoryUsingLinq().x();
        };
    }
}
