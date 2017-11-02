using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using WebApiODataService.Models;
using WebApiODataService.Repositories;

namespace WebApiODataService.Controllers
{
    public class TripsController : ODataController
    {
        private TripRepository _tripRepository;

        public TripsController()
        {
            _tripRepository = new TripRepository();
        }

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_tripRepository.GetAll().AsQueryable());
        }

        [EnableQuery]
        public SingleResult<Trip> Get([FromODataUri] string key)
        {
            var result = _tripRepository.GetAll().Where(m => m.ID == key).AsQueryable();
            return SingleResult.Create(result);
        }

        /// <summary>
        /// Creates a new trip. 
        /// Use the POST http verb.
        /// Set Content-Type:Application/Json
        /// Set body as: {"ID":"100","Name":"New Added Trip 100"}
        /// </summary>
        public IHttpActionResult Post([FromBody] Trip trip)
        {
            return Ok<Trip>(AddOrUpdate(trip));
        }

        /// <summary>
        /// Creates or update new trips
        /// POST: /Trips/DemoService.AddOrUpdate
        /// Set Content-Type:Application/Json
        /// Set body as: {"data":[{"ID":"101","Name":"New Added Trip 101"},{"ID":"102","Name":"New Added Trip 102"}]}
        /// </summary>
        [HttpPost]
        public IHttpActionResult AddOrUpdate(ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var trips = (IEnumerable<Trip>)parameters["data"];
            foreach(var trip in trips)
            {
                AddOrUpdate(trip);
            }
            return Ok();
        }

        private Trip AddOrUpdate(Trip trip)
        {
            return _tripRepository.AddOrUpdate(trip);
        }
    }
}