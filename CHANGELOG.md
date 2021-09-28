@@@ = breaking change


# 2.1.0

- @@@ Switch from Newtonsoft.Json to System.Text.Json
- @@@ Remove `PrivateStateContractResolver`
- Add `TrimStringsJsonConverter`
- `Auditor`: fix `NullReferenceException` with nested nullable value objects