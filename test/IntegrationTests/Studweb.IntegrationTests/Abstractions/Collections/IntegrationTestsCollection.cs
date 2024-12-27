namespace Studweb.IntegrationTests.Abstractions.Collections;

[CollectionDefinition("IntegrationTests", DisableParallelization = true)]
public class IntegrationTestsCollection : ICollectionFixture<IntegrationTestWebAppFactory>
{
}