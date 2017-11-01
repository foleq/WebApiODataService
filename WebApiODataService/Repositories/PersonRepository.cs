using System.Collections.Generic;
using System.Linq;
using WebApiODataService.DataSource;
using WebApiODataService.Models;

namespace WebApiODataService.Repositories
{
    public class PersonRepository
    {
        private TripRepository _tripRepository;

        public PersonRepository()
        {
            _tripRepository = new TripRepository();
        }

        public List<Person> GetAll()
        {
            return DemoDataSources.Instance.People;
        }

        public Person AddOrUpdate(Person model)
        {
            var existingModel = DemoDataSources.Instance.People
                .FirstOrDefault(m => m.ID == model.ID);

            UpdateTrips(model);
            
            if (existingModel == null)
            {
                DemoDataSources.Instance.People.Add(model);
                return model;
            }
            else
            {
                existingModel.Name = model.Name;
                existingModel.Description = model.Description;
                existingModel.Trips = model.Trips;
                return existingModel;
            }
        }

        private void UpdateTrips(Person person)
        {
            var trips = new List<Trip>();
            if(person.Trips != null)
            {
                foreach (var trip in person.Trips)
                {
                    trips.Add(_tripRepository.AddOrUpdate(trip));
                }
            }
            person.Trips = trips;
        }
    }
}