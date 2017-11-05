using Machine.Specifications;
using ODataClient.Helpers;
using ODataClient.Entities;
using ODataClient.Builders;
using ODataClient.Models;
using ODataClient.Configurations;
using System.Collections.Generic;

namespace ODataClient.Tests
{
    class TestsOnExistingODataServiceSpecs
    {
        It should_work_with_odatarepository = () =>
        {
            var result = _personRepository.GetCollection(new ODataSearchParameters
            {
                Filter = new ODataFilter()
                {
                    Query = "Trips/any(x: x/ID eq '0003')",
                },
                Top = 2
            });
        };
        
        It should_addOrUpdate_with_odatarepository = () =>
        {
            var result = _tripRepository.AddOrUpdate(new List<Trip>()
            {
                new Trip()
                {
                    ID = "98",
                    Name = "Name 98",
                },
                new Trip()
                {
                    ID = "99",
                    Name = "Name 99",
                }
            });
            result.ShouldBeTrue();
        };

        It should_test = () =>
        {
            new TestRepositoryUsingLinq().x();
        };

        private static readonly IODataRepository<Person> _personRepository = 
            new ODataRepository<Person>(new HttpHandler(), new ODataUrlBuilder<Person>(new PersonRepositoryConfiguration()));
        private static readonly IODataRepository<Trip> _tripRepository =
                new ODataRepository<Trip>(new HttpHandler(), new ODataUrlBuilder<Trip>(new TripRepositoryConfiguration()));
    }
}
