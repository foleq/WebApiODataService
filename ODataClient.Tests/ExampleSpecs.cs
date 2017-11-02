using Machine.Specifications;


namespace ODataClient.Tests
{
    public class EntitiesRepositorySpecs
    {
        It should_return_true = () =>
        {
            new EntitiesRepository().x();
            true.ShouldBeTrue();
        };
    }
}