# Alpha.Maintenance.Core.Tests

This project contains unit tests for the Alpha.Maintenance.Core library models and business logic.

## Project Structure

```
Alpha.Maintenance.Core.Tests/
├── Model/
│   └── ResetPotentialWinLimitJobConfig/
│       └── ResetPotentialWinLimitJobConfigModelTests.cs
├── Properties/
│   └── AssemblyInfo.cs
├── Alpha.Maintenance.Core.Tests.csproj
├── app.config
├── packages.config
└── README.md
```

## Test Coverage

### ResetPotentialWinLimitJobConfigModel

The `ResetPotentialWinLimitJobConfigModelTests` class provides comprehensive test coverage for the `ResetPotentialWinLimitJobConfigModel` class, including:

#### Property Tests
- **DefaultTime**: Tests setting and retrieving the default time string
- **IsActive**: Tests setting and retrieving the active status boolean
- **ManualTime**: Tests setting and retrieving the manual time DateTime (including null values)

#### Computed Property Tests
- **DefaultManualTime**: Tests that it returns the current date as a long date string
- **HasManualTime**: Tests various scenarios:
  - Returns `false` when ManualTime is null
  - Returns `false` when ManualTime is in the past
  - Returns `true` when ManualTime is current or in the future
- **DisplayedManualTime**: Tests various scenarios:
  - Returns empty string when ManualTime is null or in the past
  - Returns formatted date when ManualTime is current or in the future
  - Verifies that seconds are removed from the displayed time

#### Method Tests
- **ToString()**: Tests JSON formatting with various property combinations:
  - All properties set
  - ManualTime is null
  - DefaultTime is null
  - All properties are null/empty

#### Edge Case Tests
- DateTime at midnight
- DateTime at end of day
- Multiple calls to computed properties
- DateTime with seconds and milliseconds

## Dependencies

- **NUnit 3.7.1**: Testing framework
- **AutoFixture 3.50.3**: Test data generation
- **AutoFixture.NUnit3 3.50.3**: NUnit integration for AutoFixture
- **NSubstitute 2.0.3**: Mocking framework
- **CompareNETObjects 3.09.0.0**: Object comparison utilities
- **Newtonsoft.Json 13.0.3**: JSON handling

## Running Tests

The tests can be run using:
- Visual Studio Test Explorer
- NUnit Test Runner
- Command line with NUnit console runner

## Test Naming Convention

Tests follow the pattern: `MethodName_Scenario_ExpectedResult`

Examples:
- `HasManualTime_WhenManualTimeIsNull_ReturnsFalse`
- `DisplayedManualTime_WhenManualTimeIsInFuture_ReturnsFormattedDate`
- `ToString_WhenAllPropertiesAreSet_ReturnsCorrectJsonFormat`

## Adding New Tests

When adding tests for new models in the Core library:

1. Create a new folder under `Model/` for the model's namespace
2. Create a test class following the naming convention: `{ModelName}Tests.cs`
3. Follow the existing test structure with Arrange-Act-Assert pattern
4. Include comprehensive coverage for all properties, methods, and edge cases
5. Update this README with the new test coverage information 