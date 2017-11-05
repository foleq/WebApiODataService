namespace ODataClient.Configurations
{
    public interface IRepositoryConfiguration<TModel> 
        where TModel: class
    {
        string ODataEndpointUrl { get; }
        string EntitySetName { get; }
        string AddOrUpdateAction { get; }
    }

    public abstract class RepositoryConfiguration<TModel> : IRepositoryConfiguration<TModel>
        where TModel : class
    {
        public string ODataEndpointUrl => "http://localhost.odataservice/";

        public abstract string EntitySetName { get; }

        public string AddOrUpdateAction => "DemoService.AddOrUpdate";
    }
}
