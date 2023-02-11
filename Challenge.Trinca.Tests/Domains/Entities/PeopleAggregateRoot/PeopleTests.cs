using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.Errors;
using Challenge.Trinca.Domain.AggregatesRoot.PeopleAggregateRoot.ValueObjects.Enums;
using Challenge.Trinca.Domain.Common.Exceptions;
using Challenge.Trinca.Tests.Unit.BaseFixtures;

namespace Challenge.Trinca.Tests.Unit.Domains.Entities.PeopleAggregateRoot;

public sealed class PeopleTests
{
    [Fact(DisplayName = "People.Create() should return new person when valid parameters is given")]
    [Trait("Domain", "People - Create")]
    public void Create_ReturnValidPeople()
    {
        // Arrange
        var examplePeople = CommonPeopleFixture.GetPeople();

        // Act
        var person = People.Create(examplePeople.Name, examplePeople.IsCoOwner);

        // Assert 
        person.Should().NotBeNull();
        person.Name.Should().Be(examplePeople.Name);
        person.IsCoOwner.Should().Be(examplePeople.IsCoOwner);
        person.Id.Should().NotBeEmpty();
        person.Invites.Should().BeEmpty();

        person.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        person.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact(DisplayName = "People.CreateWithId() should return new person when valid parameters is given")]
    [Trait("Domain", "People - CreateWithId")]
    public void CreateWithId_ReturnValidPeople()
    {
        // Arrange
        var examplePeople = CommonPeopleFixture.GetPeople();

        // Act
        var person = People.CreateWithId(examplePeople.Id.ToString(), examplePeople.Name, examplePeople.IsCoOwner);

        // Assert 
        person.Should().NotBeNull();
        person.Name.Should().Be(examplePeople.Name);
        person.IsCoOwner.Should().Be(examplePeople.IsCoOwner);
        person.Id.Should().Be(examplePeople.Id);
        person.Invites.Should().BeEmpty();

        person.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        person.CreatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact(DisplayName = "People.Create() should throw NameNullOrWhiteSpace error when name is null or white space")]
    [Trait("Domain", "People - Create")]
    public void Create_ThrowNameNullOrWhiteSpaceErrorWhenCreating()
    {
        // Arrange
        var personName = CommonPeopleFixture.GetNullOrWhiteSpacePeopleName();
        var personIsCoOwner = CommonPeopleFixture.GetRandomBool();

        // Act
        var action = () => People.Create(personName, personIsCoOwner);

        // Assert 
        action.Should()
            .ThrowExactly<EntityValidationException>()
            .WithMessage(PeopleErrors.NameNullOrWhiteSpace.Description);
    }

    [Fact(DisplayName = "People.Create() should throw NameMaxLength error when name is null")]
    [Trait("Domain", "People - Create")]
    public void Create_ThrowNameMaxLengthErrorWhenCreating()
    {
        // Arrange
        var invalidPeopleName = CommonPeopleFixture.GetInvalidPeopleName();
        var personIsCoOwner = CommonPeopleFixture.GetRandomBool();

        // Act
        var action = () => People.Create(invalidPeopleName, personIsCoOwner);

        // Assert 
        action.Should()
            .ThrowExactly<EntityValidationException>()
            .WithMessage(PeopleErrors.NameMaxLength.Description);
    }

    [Fact(DisplayName = "People.Invite() should add invite")]
    [Trait("Domain", "People - Invite")]
    public void Invite_ShouldAddInvite()
    {
        // Arrange
        var examplePeople = CommonPeopleFixture.GetPeople();
        var exampleInvite = CommonPeopleFixture.GetInvite();

        // Act
        examplePeople.Invite(exampleInvite);

        // Assert 
        examplePeople.Invites.Should().ContainSingle();
        examplePeople.UpdatedDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact(DisplayName = "People.AcceptInvite() should accept invite")]
    [Trait("Domain", "People - AcceptInvite")]
    public void AcceptInvite_ShouldAcceptInvite()
    {
        // Arrange
        var examplePeople = CommonPeopleFixture.GetPeople();
        var exampleInvite = CommonPeopleFixture.GetInvite();
        var isVegetarian = CommonPeopleFixture.GetRandomBool();

        examplePeople.Invite(exampleInvite);

        // Act
        examplePeople.AcceptInvite(exampleInvite, isVegetarian);

        // Assert 
        exampleInvite.Status.Should().Be(InviteStatus.Accepted);
        examplePeople.GetDomainEvents().Should().ContainSingle();
    }

    [Fact(DisplayName = "People.DeclineInvite() should accept invite")]
    [Trait("Domain", "People - DeclineInvite")]
    public void DeclineInvite_ShouldDeclineInvite()
    {
        // Arrange
        var examplePeople = CommonPeopleFixture.GetPeople();
        var exampleInvite = CommonPeopleFixture.GetInvite();

        examplePeople.Invite(exampleInvite);

        // Act
        examplePeople.DeclineInvite(exampleInvite);

        // Assert 
        exampleInvite.Status.Should().Be(InviteStatus.Declined);
        examplePeople.GetDomainEvents().Should().ContainSingle();
    }
}
