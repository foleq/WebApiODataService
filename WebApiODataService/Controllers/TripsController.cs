using System.Linq;
using System.Web.Http;
using System.Web.OData;
using WebApiODataService.DataSource;

namespace WebApiODataService.Controllers
{
    [EnableQuery]
    public class TripsController : ODataController
    {
        public IHttpActionResult Get()
        {
            return Ok(DemoDataSources.Instance.Trips.AsQueryable());
        }
    }
}