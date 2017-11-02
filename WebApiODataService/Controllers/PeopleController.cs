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
    public class PeopleController : ODataController
    {
        private PersonRepository _personRepository;

        public PeopleController()
        {
            _personRepository = new PersonRepository();
        }

        public IHttpActionResult Get()
        {
            return Ok(_personRepository.GetAll().AsQueryable());
        }

        public SingleResult<Person> Get([FromODataUri] string key)
        {
            var result = _personRepository.GetAll().Where(m => m.ID == key).AsQueryable();
            return SingleResult.Create(result);
        }

        /// <summary>
        /// Creates a new trip. 
        /// Use the POST http verb.
        /// Set Content-Type:Application/Json
        /// Set body as: { "ID":"5","Name":"John Wall","Description":"very old man","Trips":["ID":"5","Name":"New Trip 5"] }
        /// </summary>
        public IHttpActionResult Post([FromBody] Person person)
        {
            try
            {
                return Ok<Person>(_personRepository.AddOrUpdate(person));
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