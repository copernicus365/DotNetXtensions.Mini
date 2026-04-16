# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build

# Run all tests
dotnet test Tests/

# Run a single test by name
dotnet test Tests/ --filter "FullyQualifiedName~TestMethodName"

# Generate the combined single-file output (DnxMini.cs)
dotnet script combine.cs
```

## Architecture

This is a small utility library (`DotNetXtensions.Mini`) targeting net10.0, published as a NuGet package. It provides extension methods and helpers across several static classes, all under the `DotNetXtensions` namespace.

**Library source** lives in `DotNetXtensions.Mini/`. Each static class has its own subfolder with partial class files (e.g. `XString/XString_Trim.cs`, `XDateTimes/XDateTimes_Round.cs`). The classes are:

- `XChar` — char extension methods (ASCII checks, conversions)
- `XString` — string extensions: trim helpers, `SubstringMax`, `ToValue` parse methods (`ToInt`, `ToLong`, `ToBool`, etc.), `Print`
- `XNewLines` — line splitting (`GetLines`, `GetLinesLazy`, `ForEachLine`)
- `XLinq` — null-safe collection extensions (`IsEmpty`, `NotEmpty`, `CountN`, etc.) and conditional LINQ (`WhereIf`, `SkipIf`, `TakeIf`); uses C# `extension` blocks (new syntax)
- `XDateTimes` — `DateTime`/`DateTimeOffset` extensions: Unix epoch conversions, offset conversions, rounding
- `XNumber` — generic `InRange`/`NotInRange` using `INumber<T>`
- `XDictionary` — `DictionariesAreEqual`
- `PathX` — path utilities (forward-slash normalization)

**`DnxMini.cs`** is a generated single-file amalgamation of all source files, produced by running `combine.cs` (a `dotnet script`). Do not edit it directly — edit the individual source files and regenerate.

**Tests** in `Tests/` use xUnit. `globals.cs` sets up global usings including `Xunit.Assert` as static and `TestsX` helpers. Test files mirror the source structure (e.g. `Tests/XString/`, `Tests/XDateTimes/`).
