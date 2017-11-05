using ODataClient.Entities;

namespace ODataClient.Configurations
{
    public class TripRepositoryConfiguration : RepositoryConfiguration<Trip>
    {
        public override string EntitySetName => "Trips";
    }
}
