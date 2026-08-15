
using API;

namespace Tests.Fixtures;

public class IntegrationTestFixture 
{
    public CustomWebApplicationFactory<Program> Factory { get; }
    public IntegrationTestFixture()
    {
        Factory = new CustomWebApplicationFactory<Program>();
    }
}
