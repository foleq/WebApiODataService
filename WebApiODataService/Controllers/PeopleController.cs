using System.Collections.Generic;
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
        /// Set body as: { "ID":"100","Name":"John Wall 100","Description":"my number is 100","Trips":[{"ID":"200","Name":"New Trip 200", "ID":"201","Name":"New Trip 201"}]}
        /// </summary>
        public IHttpActionResult Post([FromBody] Person person)
        {
            return Ok<Person>(AddOrUpdate(person));
        }

        /// <summary>
        /// Creates or update new trips
        /// POST: /People/DemoService.AddOrUpdate
        /// Set Content-Type:Application/Json
        /// Set body as: {"data":[{"ID":"101","Name":"John Wall 101","Description":"my number is 101","Trips":[{"ID":"201","Name":"New Trip 201", "ID":"202","Name":"New Trip 202"}]},{"ID":"102","Name":"John Wall 102","Description":"my number is 102"}]}
        /// </summary>
        [HttpPost]
        public IHttpActionResult AddOrUpdate(ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var people = (IEnumerable<Person>)parameters["data"];
            foreach (var person in people)
            {
                AddOrUpdate(person);
            }
            return Ok();
        }

        private Person AddOrUpdate(Person person)
        {
            return _personRepository.AddOrUpdate(person);
        }
    }
}