using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using WebApiODataService.Models;
using WebApiODataService.Repositories;

namespace WebApiODataService.Controllers
{
    [EnableQuery]
    public class TripsController : ODataController
    {
        private TripRepository _tripRepository;

        public TripsController()
        {
            _tripRepository = new TripRepository();
        }

        public IHttpActionResult Get()
        {
            return Ok(_tripRepository.GetAll().AsQueryable());
        }

        public SingleResult<Trip> Get([FromODataUri] string key)
        {
            var result = _tripRepository.GetAll().Where(m => m.ID == key).AsQueryable();
            return SingleResult.Create(result);
        }

        /// <summary>
        /// Creates a new trip. 
        /// Use the POST http verb.
        /// Set Content-Type:Application/Json
        /// Set body as: { "ID":"4","Name":"New Trip" }
        /// </summary>
        public IHttpActionResult Post([FromBody] Trip trip)
        {
            try
            {
                return Ok<Trip>(_tripRepository.AddOrUpdate(trip));
            }
            catch (ArgumentNullException e)
            {
                Debugger.Log(1, "Error", e.Message);
                return BadRequest();
            }
            catch (ArgumentException e)
            {
                Debugger.Log(1, "Error", e.Message);
                return BadRequest();
            }
            catch (InvalidOperationException e)
            {
                Debugger.Log(1, "Error", e.Message);
                return Conflict();
            }
        }
    }
}