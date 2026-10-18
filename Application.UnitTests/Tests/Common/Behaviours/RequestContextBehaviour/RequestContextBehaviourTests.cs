using Application.Common.Behaviours.RequestContextBehaviour;
using Application.Common.Contexts;
using Application.Repositories.IdentityRepositories;
using Application.UnitTests.Utils.Models.Requests;
using Domain.Entities.IdentityEntities;
using FluentAssertions;
using NSubstitute;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Constants;
using Utils.Exceptions.CustomExceptions;

namespace Application.UnitTests.Tests.Common.Behaviours.RequestContextBehaviour;

public class RequestContextBehaviourTests
{
    /* - - - Method: Handle - - - */
    [Fact]
    public async Task Handle_WithClinicNotFound_ThrowsAuthorizationFailedExceptionAndDoesNotCallNext()
    {
        // Arrange
        var clinicId = Guid.NewGuid();
        var requestContext = Substitute.For<IRequestContext>();
        requestContext.ClinicId.Returns(clinicId);
        
        var clinicRepository = Substitute.For<IClinicRepository>();
        clinicRepository
            .GetByIdAsync(clinicId, Arg.Any<CancellationToken>())
            .Returns((Clinic?)null);
        var userRepository = Substitute.For<IUserRepository>();
        
        var sut = new RequestContextBehaviour<TestRequestWithRequestContext, TestResponse>(
            requestContext,
            clinicRepository,
            userRepository);
        
        var nextCalled = false;
        Task<TestResponse> Next(CancellationToken _)
        {
            nextCalled = true;
            return Task.FromResult(new TestResponse());
        }
        
        // Act
        var act = () => sut.Handle(new TestRequestWithRequestContext(), Next, CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<AuthorizationFailedException>();
        nextCalled.Should().BeFalse();

        await clinicRepository
            .Received(1)
            .GetByIdAsync(clinicId, Arg.Any<CancellationToken>());
        await userRepository
            .DidNotReceive()
            .GetByClinicIdAndUserIdAsync(
                Arg.Any<Guid>(), 
                Arg.Any<Guid>(),
                Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Handle_WithUserNotFound_ThrowsAuthorizationFailedExceptionAndDoesNotCallNext()
    {
        // Arrange
        var clinic = TestClinicBuilder.Create().Build();
        var user = TestUserBuilder.Create(clinic).Build();
        
        var requestContext = Substitute.For<IRequestContext>();
        requestContext.ClinicId.Returns(clinic.Id);
        requestContext.UserId.Returns(user.Id);
        
        var clinicRepository = Substitute.For<IClinicRepository>();
        clinicRepository
            .GetByIdAsync(clinic.Id, Arg.Any<CancellationToken>())
            .Returns(clinic);
        
        var userRepository = Substitute.For<IUserRepository>();
        userRepository
            .GetByClinicIdAndUserIdAsync(clinic.Id, user.Id, Arg.Any<CancellationToken>())
            .Returns((User?)null);
        
        var sut = new RequestContextBehaviour<TestRequestWithRequestContext, TestResponse>(
            requestContext,
            clinicRepository,
            userRepository);
        
        var nextCalled = false;
        Task<TestResponse> Next(CancellationToken _)
        {
            nextCalled = true;
            return Task.FromResult(new TestResponse());
        }
        
        // Act
        var act = () => sut.Handle(new TestRequestWithRequestContext(), Next, CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<AuthorizationFailedException>();
        nextCalled.Should().BeFalse();
        
        await clinicRepository
            .Received(1)
            .GetByIdAsync(clinic.Id, Arg.Any<CancellationToken>());
        await userRepository
            .Received(1)
            .GetByClinicIdAndUserIdAsync(clinic.Id, user.Id, Arg.Any<CancellationToken>());
    }
    
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Handle_WithArchivedOrDeletedUser_ThrowsAuthorizationFailedExceptionAndDoesNotCallNext(bool isDeleted)
    {
        // Arrange
        var currentDateTime = TestConstants.DefaultCurrentDateTime;
        var clinic = TestClinicBuilder.Create().Build();
        var user = TestUserBuilder.Create(clinic).Build();
        user.Archive(currentDateTime);
        if (isDeleted)
        {
            user.Delete();
        }
        
        var requestContext = Substitute.For<IRequestContext>();
        requestContext.ClinicId.Returns(clinic.Id);
        requestContext.UserId.Returns(user.Id);
        
        var clinicRepository = Substitute.For<IClinicRepository>();
        clinicRepository
            .GetByIdAsync(clinic.Id, Arg.Any<CancellationToken>())
            .Returns(clinic);
        
        var userRepository = Substitute.For<IUserRepository>();
        userRepository
            .GetByClinicIdAndUserIdAsync(clinic.Id, user.Id, Arg.Any<CancellationToken>())
            .Returns(user);
        
        var sut = new RequestContextBehaviour<TestRequestWithRequestContext, TestResponse>(
            requestContext,
            clinicRepository,
            userRepository);
        
        var nextCalled = false;
        Task<TestResponse> Next(CancellationToken _)
        {
            nextCalled = true;
            return Task.FromResult(new TestResponse());
        }
        
        // Act
        var act = () => sut.Handle(new TestRequestWithRequestContext(), Next, CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<AuthorizationFailedException>();
        nextCalled.Should().BeFalse();
        
        await clinicRepository
            .Received(1)
            .GetByIdAsync(clinic.Id, Arg.Any<CancellationToken>());
        await userRepository
            .Received(1)
            .GetByClinicIdAndUserIdAsync(clinic.Id, user.Id, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Handle_WithRequestNotRequiringContext_SkipsValidationAndCallsNext()
    {
        // Arrange
        var response = new TestResponse();
        var requestContext = Substitute.For<IRequestContext>();
        var clinicRepository = Substitute.For<IClinicRepository>();
        var userRepository = Substitute.For<IUserRepository>();
        
        var sut = new RequestContextBehaviour<TestRequest, TestResponse>(
            requestContext,
            clinicRepository,
            userRepository);
        
        var nextCalled = false;
        Task<TestResponse> Next(CancellationToken _)
        {
            nextCalled = true;
            return Task.FromResult(response);
        }
        
        // Act
        var result = await sut.Handle(new TestRequest(), Next, CancellationToken.None);
        
        // Assert
        nextCalled.Should().BeTrue();
        result.Should().Be(response);
        
        await clinicRepository
            .DidNotReceive()
            .GetByIdAsync(
                Arg.Any<Guid>(), 
                Arg.Any<CancellationToken>());
        await userRepository
            .DidNotReceive()
            .GetByClinicIdAndUserIdAsync(
                Arg.Any<Guid>(), 
                Arg.Any<Guid>(),
                Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Handle_WithValidClinicAndUser_PopulatesRequestAndCallsNext()
    {
        // Arrange
        var clinic = TestClinicBuilder.Create().Build();
        var user = TestUserBuilder.Create(clinic).Build();
        
        var requestContext = Substitute.For<IRequestContext>();
        requestContext.ClinicId.Returns(clinic.Id);
        requestContext.UserId.Returns(user.Id);
        
        var clinicRepository = Substitute.For<IClinicRepository>();
        clinicRepository
            .GetByIdAsync(clinic.Id, Arg.Any<CancellationToken>())
            .Returns(clinic);
        
        var userRepository = Substitute.For<IUserRepository>();
        userRepository
            .GetByClinicIdAndUserIdAsync(clinic.Id, user.Id, Arg.Any<CancellationToken>())
            .Returns(user);
        
        var sut = new RequestContextBehaviour<TestRequestWithRequestContext, TestResponse>(
            requestContext,
            clinicRepository,
            userRepository);
        
        var request = new TestRequestWithRequestContext();
        var response = new TestResponse();
        var nextCalled = false;
        Task<TestResponse> Next(CancellationToken _)
        {
            nextCalled = true;
            return Task.FromResult(response);
        }
        
        // Act
        var result = await sut.Handle(request, Next, CancellationToken.None);
        
        // Assert
        nextCalled.Should().BeTrue();
        result.Should().Be(response);

        request.Clinic.Should().Be(clinic);
        request.User.Should().Be(user);
        
        await clinicRepository
            .Received(1)
            .GetByIdAsync(clinic.Id, Arg.Any<CancellationToken>());
        await userRepository
            .Received(1)
            .GetByClinicIdAndUserIdAsync(clinic.Id, user.Id, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Handle_WithValidClinicAndUser_ForwardsCancellationTokenToRepositoriesAndNext()
    {
        // Arrange
        var clinic = TestClinicBuilder.Create().Build();
        var user = TestUserBuilder.Create(clinic).Build();
        
        var requestContext = Substitute.For<IRequestContext>();
        requestContext.ClinicId.Returns(clinic.Id);
        requestContext.UserId.Returns(user.Id);
        
        var clinicRepository = Substitute.For<IClinicRepository>();
        clinicRepository
            .GetByIdAsync(clinic.Id, Arg.Any<CancellationToken>())
            .Returns(clinic);
        
        var userRepository = Substitute.For<IUserRepository>();
        userRepository
            .GetByClinicIdAndUserIdAsync(clinic.Id, user.Id, Arg.Any<CancellationToken>())
            .Returns(user);
        
        var sut = new RequestContextBehaviour<TestRequestWithRequestContext, TestResponse>(
            requestContext,
            clinicRepository,
            userRepository);
        
        using var cancellationTokenSource = new CancellationTokenSource();
        var expectedCancellationToken = cancellationTokenSource.Token;
        CancellationToken? receivedCancellationToken = null;
        
        Task<TestResponse> Next(CancellationToken cancellationToken)
        {
            receivedCancellationToken = cancellationToken;
            return Task.FromResult(new TestResponse());
        }
        
        // Act
        await sut.Handle(new TestRequestWithRequestContext(), Next, expectedCancellationToken);
        
        // Assert
        receivedCancellationToken.Should().Be(expectedCancellationToken);
        await clinicRepository
            .Received(1)
            .GetByIdAsync(clinic.Id, expectedCancellationToken);
        await userRepository
            .Received(1)
            .GetByClinicIdAndUserIdAsync(clinic.Id, user.Id, expectedCancellationToken);
    }
}