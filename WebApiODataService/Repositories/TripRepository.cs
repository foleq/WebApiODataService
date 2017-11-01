using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiODataService.DataSource;
using WebApiODataService.Models;

namespace WebApiODataService.Repositories
{

    public class TripRepository
    {
        public List<Trip> GetAll()
        {
            return DemoDataSources.Instance.Trips;
        }

        public Trip AddOrUpdate(Trip model)
        {
            var existingModel = DemoDataSources.Instance.Trips
                .FirstOrDefault(m => m.ID == model.ID);
            if(existingModel == null)
            {
                DemoDataSources.Instance.Trips.Add(model);
                return model;
            }
            else
            {
                existingModel.Name = model.Name;
                return existingModel;
            }
        }
    }
}