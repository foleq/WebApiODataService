using DemoService;
using Microsoft.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiODataService.Models;

namespace ODataClient
{
    public class TestRepositoryUsingLinq
    {
        public void x()
        {
            //var odataserviceUrl = "http://localhost:54484/";
            var odataserviceUrl = "http://localhost.odataservice/";

            var context = new DemoContainer(new Uri(odataserviceUrl));
            var people = context.People.Expand(c => c.Trips);
            var test = context.People.Where(p => p.Name == "test");
            var test2 = context.People.Where(p => p.Trips.Any(t => t.ID == "0001"));
            var result = test2.ToList();
            //TODO: Deep insert doesnt work :(
            //DataServiceCollection<Trip> trips =
            //    new DataServiceCollection<Trip>(context, "Trips"/*entityset name*/, null, null);
            //trips.Add(Trip.CreateTrip("55", "name 55"));
            //trips.Load(Trip.CreateTrip("55", "name 55"));
            var trips = new DataServiceCollection<Trip>(new List<Trip>() { Trip.CreateTrip("55", "name 55") }, TrackingMode.None);

            var addOrUpdateQuery = context.People.AddOrUpdate(new List<Person>()
            {
                new Person()
                {
                    ID = "99",
                    Name = "name 99",
                    Description = "desc 99",
                    Trips = trips
                }
            });
            var r = addOrUpdateQuery.Execute();
        }


    }
}
