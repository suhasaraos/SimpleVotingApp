# VotingApp Unit Tests

This project contains comprehensive unit tests for the VotingApp application.

## Test Coverage

### Services Tests (`VoteStorageTests.cs`)
Tests for the `VoteStorage` service that manages vote data:
- ✅ Adding valid votes increases count
- ✅ Adding invalid votes doesn't affect counts
- ✅ Getting vote counts returns all options
- ✅ Vote counts return new dictionary instances (defensive copying)
- ✅ Getting options returns all option names
- ✅ Multiple votes increase count correctly
- ✅ Different options increment respective counts

### Controllers Tests (`VotingControllerTests.cs`)
Tests for the `VotingController` that handles HTTP requests:
- ✅ Index action returns empty vote counts when user hasn't voted
- ✅ Index action returns vote counts when user has voted
- ✅ Vote action adds vote and sets session for valid option
- ✅ Vote action ignores votes when user already voted
- ✅ Vote action ignores empty options
- ✅ Vote action ignores null options
- ✅ Vote action always redirects to Index
- ✅ Error action returns error view with request ID

### Models Tests (`VoteViewModelTests.cs`)
Tests for the `VoteViewModel` data model:
- ✅ Default constructor initializes properties correctly
- ✅ SelectedOption property can be set
- ✅ HasVoted property can be set
- ✅ VoteCounts dictionary can be set

## Running the Tests

### Run all tests:
```powershell
dotnet test VotingApp.Tests\VotingApp.Tests.csproj
```

### Run tests with detailed output:
```powershell
dotnet test VotingApp.Tests\VotingApp.Tests.csproj --verbosity detailed
```

### Run tests with code coverage:
```powershell
dotnet test VotingApp.Tests\VotingApp.Tests.csproj --collect:"XPlat Code Coverage"
```

## Test Framework

- **xUnit** - Testing framework
- **Moq** - Mocking library for creating test doubles
- **Microsoft.AspNetCore.Mvc.Testing** - ASP.NET Core testing utilities

## Test Results Summary

- **Total Tests**: 19
- **Passed**: 19
- **Failed**: 0
- **Skipped**: 0

## Notes

- Tests use Moq to mock HTTP session behavior and logging dependencies
- VoteStorage uses a static dictionary, so tests may affect each other if run in parallel
- All controller tests properly mock HttpContext and Session for isolation
