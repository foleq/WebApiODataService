using Microsoft.OData.Edm;
using System;
using System.Web.Http;
using System.Web.OData.Batch;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using WebApiODataService.Models;

namespace WebApiODataService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapODataServiceRoute("odata", null, GetEdmModel(), new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer));
            config.EnsureInitialized();
        }
        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.Namespace = "Demos";
            builder.ContainerName = "DefaultContainer";
            builder.EntitySet<Person>("People")
                .EntityType
                .Select(new String[] { nameof(Person.Name) })
                .Filter(new String[] { nameof(Person.Description) })
                .Expand(new String[] { nameof(Person.Trips) });
            builder.EntitySet<Trip>("Trips");
            var edmModel = builder.GetEdmModel();
            return edmModel;
        }
    }
}
