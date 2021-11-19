@@@ = breaking change

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