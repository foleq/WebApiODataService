using ODataClient.Models;

namespace ODataClient.Configurations
{
    public class PersonRepositoryConfiguration : RepositoryConfiguration<Person>
    {
        public override string EntitySetName => "People";
    }
}
