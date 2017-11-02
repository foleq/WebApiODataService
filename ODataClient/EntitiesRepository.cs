using DemoService;
using Microsoft.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiODataService.Models;

namespace ODataClient
{
    public class EntitiesRepository
    {
        public void x()
        {
            var context = new DemoContainer(new Uri("http://localhost:54484/"));
            var people = context.People.Expand(c => c.Trips).ToList();

            //TODO: Deep insert doesnt work :(
            //DataServiceCollection<Trip> trips =
            //    new DataServiceCollection<Trip>(context, "Trips"/*entityset name*/, null, null);
            //trips.Add(Trip.CreateTrip("55", "name 55"));
            //trips.Load(Trip.CreateTrip("55", "name 55"));
            var trips = new DataServiceCollection<Trip>(new List<Trip>() { Trip.CreateTrip("55", "name 55") }, TrackingMode.None);

            context.People.AddOrUpdate(new List<Person>()
            {
                new Person()
                {
                    ID = "99",
                    Name = "name 99",
                    Description = "desc 99",
                    Trips = trips
                }
            }).Execute();
        }


    }
}
