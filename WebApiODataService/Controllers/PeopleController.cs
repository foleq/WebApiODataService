using System.Linq;
using System.Web.Http;
using System.Web.OData;
using WebApiODataService.DataSource;

namespace WebApiODataService.Controllers
{
    [EnableQuery]
    public class PeopleController : ODataController
    {
        public IHttpActionResult Get()
        {
            return Ok(DemoDataSources.Instance.People.AsQueryable());
        }
    }
}