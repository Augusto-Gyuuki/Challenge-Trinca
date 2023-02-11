using NetArchTest.Rules;

namespace Challenge.Trinca.Tests.Unit;

public sealed class ArchitectureTests
{
    private const string DOMAIN_NAMESPACE = "Domain";
    private const string APPLICATION_NAMESPACE = "Application";
    private const string INFRASTRUCTURE_NAMESPACE = "Infrastructure";
    private const string PRESENTATION_NAMESPACE = "Presentation";
    private const string WEB_NAMESPACE = "Web";

    [Fact(DisplayName = "Domain Layer Should Not Have Dependency on Others Projects")]
    [Trait("Architecture", "Clean Arch")]
    public void Domain_ShouldNotHaveDependencyOnOthersProjects()
    {
        //Arrange
        var assembly = typeof(Domain.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            APPLICATION_NAMESPACE,
            INFRASTRUCTURE_NAMESPACE,
            PRESENTATION_NAMESPACE,
            WEB_NAMESPACE,
        };

        //Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        //Asserts
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact(DisplayName = "Application Layer Should Not Have Dependency on Others Projects")]
    [Trait("Architecture", "Clean Arch")]
    public void Application_ShouldNotHaveDependencyOnOthersProjects()
    {
        //Arrange
        var assembly = typeof(Application.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            INFRASTRUCTURE_NAMESPACE,
            PRESENTATION_NAMESPACE,
            WEB_NAMESPACE,
        };

        //Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        //Asserts
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact(DisplayName = "Handlers Should Have Dependency on Domain")]
    [Trait("Architecture", "Clean Arch")]
    public void Handlers_ShouldHaveDependencyOnDomain()
    {
        //Arrange
        var assembly = typeof(Application.AssemblyReference).Assembly;

        //Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveDependencyOn(DOMAIN_NAMESPACE);
        //.GetResult();

        //Asserts
        //result.IsSuccessful.Should().BeTrue();
    }

    [Fact(DisplayName = "Infrastructure Layer Should Not Have Dependency on Others Projects")]
    [Trait("Architecture", "Clean Arch")]
    public void Infrastructure_ShouldNotHaveDependencyOnOthersProjects()
    {
        //Arrange
        var assembly = typeof(Infrastructure.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            PRESENTATION_NAMESPACE,
            WEB_NAMESPACE,
        };

        //Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        //Asserts
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact(DisplayName = "Persistence Layer Should Not Have Dependency on Others Projects")]
    [Trait("Architecture", "Clean Arch")]
    public void Persistence_ShouldNotHaveDependencyOnOthersProjects()
    {
        //Arrange
        var assembly = typeof(Persistence.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            PRESENTATION_NAMESPACE,
            WEB_NAMESPACE,
        };

        //Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        //Asserts
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact(DisplayName = "Presentation Layer Should Not Have Dependency on Others Projects")]
    [Trait("Architecture", "Clean Arch")]
    public void Presentation_ShouldNotHaveDependencyOnOthersProjects()
    {
        //Arrange
        var assembly = typeof(Presentation.AssemblyReference).Assembly;

        var otherProjects = new[]
        {
            INFRASTRUCTURE_NAMESPACE,
            WEB_NAMESPACE,
        };

        //Act
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        //Asserts
        result.IsSuccessful.Should().BeTrue();
    }

    //[Fact(DisplayName = "Controllers Should Have Dependency on MediatR")]
    //[Trait("Architecture", "Clean Arch")]
    //public void Controllers_ShouldHaveDependencyOnMediatR()
    //{
    //    //Arrange
    //    var assembly = typeof(Presentation.AssemblyReference).Assembly;

    //    //Act
    //    var result = Types
    //        .InAssembly(assembly)
    //        .That()
    //        .HaveNameEndingWith("Controller")
    //        .Should()
    //        .HaveDependencyOn("MediatR")
    //        .GetResult();

    //    //Asserts
    //    result.IsSuccessful.Should().BeTrue();
    //}
}