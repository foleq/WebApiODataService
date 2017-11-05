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
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
            config.MapODataServiceRoute("odata", null, GetEdmModel(), new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer));
            config.EnsureInitialized();
        }
        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.ContainerName = "DemoContainer";
            builder.Namespace = "DemoService";

            builder.EntitySet<Person>("People")
                .EntityType
                .Select(new String[] { nameof(Person.Name) })
                .Filter(new String[] { nameof(Person.Description), nameof(Person.Trips) })
                .Page(5, 5)
                .Expand(new String[] { nameof(Person.Trips) });
            builder.EntityType<Person>()
                .Collection
                .Action("AddOrUpdate")
                .CollectionEntityParameter<Person>("data");

            builder.EntitySet<Trip>("Trips")
                .EntityType
                .Page(5, 5);
            builder.EntityType<Trip>()
                .Collection
                .Action("AddOrUpdate")
                .CollectionEntityParameter<Trip>("data");

            var edmModel = builder.GetEdmModel();
            return edmModel;
        }
    }
}
