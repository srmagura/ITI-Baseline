@@@ = breaking change

## 3.0.8

- Switch from `System.Data.SqlClient` to `Microsoft.Data.SqlClient`

## 3.0.7

- Add extension method `IAuthContext.AuthorizedBelow`
- Make `IAuthContext.AnyUser` and `IAuthContext.Unauthenticated` return `Task`
- Allow specifying message when creating `NotAuthorizedException`

## 3.0.5

- `DbValueObject`: Remove `HasValue` since it is unnecessary 95% of the time

## 3.0.4

- `UnitOfWork`: Dispose data contexts immediately

## 3.0.3

- @@@ Add cancellation token to `IDomainEventHandler`

## 3.0.2

- @@@ Change `FilteredList` to use an array
- Add `ProjectToDtoArrayAsync` methods
- Don't use a nested lifetime scope for `UnitOfWork`

## 3.0.0

- @@@ Remove domain event implementation
- @@@ Refactor unit of work
- @@@ Many small refactorings

## 2.2.6

- @@@ Upgrade to .NET 6 and EF Core 6
- @@@ Refactor `IPasswordEncoder`
- @@@ `BaseDataMapConfig`: Enable constructor mapping
- @@@ `IAuthContext`: Rename `UserId` to `UserIdString`
- @@@ `EmailAddress`: Fix `value` being nullable
- @@@ `PhoneNumber`: Fix `value` being nullable
- Enable nullable in all packages

## 2.1.3

- Make `FromDbJson()` do case-insensitive property names

## 2.1.2

- @@@ Remove `FieldLengths.GeoLocation`
- @@@ Remove the `ITI.DDD.Application.Exceptions` namespace
- @@@ Remove `EntityNotFoundException`
- `DefaultPasswordEncoder`: disallow leading or trailing whitespace in passwords
- Add `OnModelCreating` methods to `AuditRecord`, `LogEntry`, and `DbRequestTrace`

## 2.1.0

- @@@ Switch from Newtonsoft.Json to System.Text.Json
- @@@ Remove `PrivateStateContractResolver`
- Add `TrimStringsJsonConverter`
- `Auditor`: fix `NullReferenceException` with nested nullable value objects
