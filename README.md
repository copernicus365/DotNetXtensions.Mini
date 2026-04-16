# DotNetXtensions.Mini

[![NuGet](https://img.shields.io/nuget/v/DotNetXtensions.Mini.svg)](https://www.nuget.org/packages/DotNetXtensions.Mini)

A lightweight, streamlined version of [DotNetXtensions](https://github.com/copernicus365/DotNetXtensions) containing the most commonly used extension methods and utilities for .NET development. This mini version provides essential string, collection, character, date/time, and path manipulation utilities without the full library overhead. It is also possible to use the single file `DnxMini.cs` directly in your project without needing to reference the NuGet package.

## Requirements

**.NET 10.0** — This library targets .NET 10 exclusively, due to the copious use of **C# 14 extension properties**. Thank you .NET for these!

## Installation

Add the NuGet package to your project:

```bash
dotnet add package DotNetXtensions.Mini
```

## Use of `DnxMini.cs` single file

You can also choose to include the `DnxMini.cs` file directly in your project without referencing the NuGet package. This allows you to use the extension methods without adding an external dependency. Note: we build this with a project script that combines all .cs classes, as well as partial classes, into a single file, so you can just include that one file in your project.

### A note on partial classes

For maintainability and for better testing, in some cases we internally have a number of separate partial classes (e.g. `XString_Trim.cs`, `XString_ToValue.cs`, etc), even though they share the same class name: `partial class XString`. That said, note that when compiling, no artifacts of partial classes remain, and also note that the nuget package is, of course, a compiled product of this source code, thus it has ZERO knowledge that partials were ever used:

> When you use partial classes in C#, the C# compiler merges all the partial declarations into one single class during compilation. After compilation, there is no trace left of the partial keyword or the fact that the class was split across files — in the resulting assembly (IL / metadata), it looks exactly like you had written one big, normal (non-partial) class from the beginning. - Grok (2026-03)

## Design Philosophy

### Graceful Null Handling

**DotNetXtensions** adopts a consistent design philosophy of gracefully handling nulls rather than throwing exceptions. This is a deliberate choice that makes the library particularly well-suited for:

- **Functional/Fluent Chaining** - No need for defensive null-checks at every step in a LINQ chain
- **Railway-Oriented Programming** - Nulls flow through pipelines without derailing them
- **Practical Utility** - For display, logging, and transformation scenarios where exceptions would add ceremony without value

```csharp
// Graceful null handling enables clean, chainable code:
string display = items?.Where(x => x.IsValid).JoinToString(", ") ?? "(none)";

// Instead of:
string display2 = items != null ? string.Join(", ", items.Where(x => x.IsValid)) : "(none)";
```

This philosophy is applied consistently throughout the library — extension members on strings, collections, and other types typically return `null` or a default value when given `null` input, rather than throwing `ArgumentNullException`.

### C# 14 Extension Properties

Members that check or transform the receiver without additional arguments are exposed as **extension properties** — no parentheses needed. Boolean state tests like `IsEmpty` and `NotEmpty` read exactly like instance properties, consistent with how .NET itself exposes state (`task.IsCompleted`, `token.IsCancellationRequested`). Methods remain methods when they accept arguments.

## Features

### String Extensions

#### Null/Empty Checks
- **`IsEmpty`** - 🌟 `true` if null or empty 🌟
- **`NotEmpty`** - 🌟 `true` if not null and not empty 🌟
- **`IsEmptyOrWhiteSpace`** - `true` if null or whitespace
- **`EmptyIfNull`** - Returns `""` if null, otherwise returns the string
- **`NullIfEmpty`** - 🌟 Returns null if empty, otherwise returns the string 🌟
- **`NullIfWhitespace`** - Returns null if null or whitespace, otherwise returns the string

```csharp
string str = "";
if(str.IsEmpty)              // true — property, no `()`
    WriteLine("Empty or null");

string name = "John";
if(name.NotEmpty)            // true
    WriteLine($"Hello {name}");

string input = "  ";
if(input.IsEmptyOrWhiteSpace) // true
    WriteLine("Whitespace only");

string result = "".NullIfEmpty;      // null
string safe   = nullStr.EmptyIfNull; // ""
```

#### String Trimming
- **`IsTrimmable`** - `true` if the string has leading or trailing whitespace
- **`TrimIfNeeded()`** - Trims only if needed; returns the **same string reference** when no trim is required
- **`TrimToNull()`** - 🌟 Trims, then returns null if the result is empty 🌟
- **`TrimN()`** - Trims if not null, returns null otherwise

```csharp
string text = "  hello  ";
string trimmed = text.TrimIfNeeded(); // "hello"

string text2 = "hello";
bool needsTrim = text2.IsTrimmable; // false

string padded = "   ";
string result = padded.TrimToNull(); // null (trimmed to empty → null)
```

#### String Utilities
- **`LengthN`** / **`CountN`** - Returns length/count or 0 if null (properties)

```csharp
string nullStr = null;
int length = nullStr.LengthN; // 0
```

#### String Conversion / Parsing Extensions

Convenient parsing methods with default values and null handling:

**Standard conversions with defaults:**
- **`ToInt()`** - Parse to int with default value on failure
- **`ToLong()`** - Parse to long with default value
- **`ToDecimal()`** - Parse to decimal with default value
- **`ToDouble()`** - Parse to double with default value
- **`ToBool()`** - Parse to bool (handles "0"/"1" as false/true)
- **`ToDateTime()`** - Parse to DateTime with optional default
- **`ToDateTimeOffset()`** - Parse to DateTimeOffset with optional default
- **`ToGuid()`** - Parse to Guid with optional default

**Nullable conversions (returns null on failure):**
- **`ToIntN()`**, **`ToLongN()`**, **`ToDecimalN()`**, **`ToDoubleN()`**
- **`ToBoolN()`**, **`ToDateTimeN()`**, **`ToDateTimeOffsetN()`**, **`ToGuidN()`**

```csharp
// With default values
int num = "42".ToInt(); // 42

string invalid = "abc";
int num2 = invalid.ToInt(-1); // -1 (default)

// Boolean parsing supports numeric strings
string enabled = "1";
bool isEnabled = enabled.ToBool(); // true
```

#### Advanced String Methods

**`FirstNotNullOrEmpty()`** - Returns first non-null/empty string from the receiver and up to two additional inputs 🌟

```csharp
string config = null;
string env = "";
string fallback = "default";

string result = config.FirstNotNullOrEmpty(env, fallback); // "default"

// Common use case: configuration fallback chain
string apiKey = Environment.GetEnvironmentVariable("API_KEY")
    .FirstNotNullOrEmpty(config["ApiKey"], "dev-key-12345");
```

**`SubstringMax()`** - Safe substring with maximum length (won't throw on out of range) 🌟

```csharp
string text = "Hello World";

// Traditional Substring would throw if length > remaining chars
string result1 = text.SubstringMax(6, 100); // "World" (no exception)

// With ellipsis for truncation
string longText = "This is a very long string that needs truncating";
string preview = longText.SubstringMax(20, "..."); // "This is a very long..."

// Try to break on word boundaries
string sentence = "The quick brown fox jumps over the lazy dog";
string excerpt = sentence.SubstringMax(20, "...", tryBreakOnWord: true);
// "The quick brown..." (breaks at word boundary)
```

### Line Processing Extensions

Efficient string splitting and enumeration by lines.

#### Line Conversion and Splitting

**`ToUnixLines()`** - Convert CRLF to LF line endings
**`GetLines()`** - Split string into array of lines (very simple, based on `string.Split`)
**`GetLinesLazy()`** - 🌟 Lazy enumeration with true deferred execution, with performant span based operations. 🌟 Perfect for LINQ operations - only processes lines that are actually consumed
**`ForEachLine()`** - Very-fast iteration, based on .NET's vectorized/SIMD-optimized `SpanLineEnumerator` (.NET 9+), but unfortunately, that doesn't allow enumeration, thus `GetLinesLazy()` can't be based on this.

```csharp
// Process until condition met, then stop

string logs = GetLargeLogFile();

string errorLine = null;
logs.ForEachLine(line => {
    if (line.StartsWith("ERROR:")) {
        errorLine = line;
        return false; // Stop processing immediately
    }
    return true; // Continue to next line
});
```

**Performance Guidance:**
- Use `GetLines()` when you need all lines as an array
- Use `GetLinesLazy()` for LINQ operations or when processing subset of lines
- Use `ForEachLine()` (.NET 9+) for fastest sequential processing with early termination

### Collection Extensions

#### Null/Empty Checks

It's hard to live without these simple few extensions! Simplifies and makes for much more fluent code.

- **`IsEmpty`** - 🌟🌟 `true` if collection/array is null or empty 🌟🌟
- **`NotEmpty`** - 🌟🌟 `true` if collection has items 🌟🌟
- **`IsNullOrEmpty`** - fuller alias for `IsEmpty` (available on arrays and `ICollection<T>`)
- **`NotNullOrEmpty`** - fuller alias for `NotEmpty` (available on arrays and `ICollection<T>`)

```csharp
List<int> numbers = new();
if (numbers.IsEmpty)  // true
    WriteLine("No numbers");

int[] items = { 1, 2, 3 };
if (items.NotEmpty)   // true
    WriteLine($"Has {items.Length} items");
```

#### Collection Utilities
- **`CountN`** / **`LengthN`** - Returns count/length or 0 if null (properties)
- **`JoinToString()`** - 🌟 Joins collection elements into a string with separator 🌟

- On `JoinToString()`, while .NET provides `string.Join()` as a static method, `JoinToString()` offers a fluent, chainable alternative that integrates seamlessly into LINQ pipelines.

```csharp
// Basic usage with default separator (comma)
var names = new[] { "Alice", "Bob", "Charlie" };
string joined = names.JoinToString(); // "Alice,Bob,Charlie"

// Custom separator
string withSemicolon = names.JoinToString("; "); // "Alice; Bob; Charlie"

// With selector - transform items inline
var users = new[] { new User("Alice", 25), new User("Bob", 30) };
string userNames = users.JoinToString(u => u.Name?.ToUpper(), "; "); // "ALICE; BOB"
```

### Character Extensions

#### ASCII Checks
High-performance character checks for ASCII characters:
- **`IsAsciiDigit()`** - Checks for 0-9
- **`IsAsciiLetter()`** - Checks for a-z or A-Z
- **`IsAsciiLower()`** - Checks for a-z
- **`IsAsciiUpper()`** - Checks for A-Z
- **`IsAsciiLetterOrDigit()`** - Checks for a-z, A-Z, or 0-9
- **`IsAsciiLowerOrDigit()`** - Checks for a-z or 0-9
- **`IsAsciiUpperOrDigit()`** - Checks for A-Z or 0-9

```csharp
char c = '5';
if (c.IsAsciiDigit()) // true
    WriteLine("It's a digit");

char letter = 'a';
if (letter.IsAsciiLower()) // true
    WriteLine("Lowercase letter");
```

#### Character Utilities
- **`IsWhitespace()`** - Extension wrapper for `char.IsWhiteSpace`
- **`IsUpper()`** / **`IsLower()`** - Extension wrappers for case checks
- **`IsNumber()`** - Extension wrapper for `char.IsNumber`
- **`ToInt()`** - Converts digit character to integer (e.g., '5' → 5) 🌟

```csharp
char digit = '7';
int num = digit.ToInt(); // 7
```

### Numeric Extensions

#### Range Checks
Generic range checking for numeric types:
- **`InRange()`** - Checks if value is within range (inclusive) 🌟
- **`NotInRange()`** - Checks if value is outside range 🌟

```csharp
int age = 25;
if (age.InRange(18, 65)) // true
    WriteLine("Working age");

double temp = 15.5;
if (temp.NotInRange(20.0, 30.0)) // true
    WriteLine("Outside comfort zone");

string name = "John";
if (name.LengthInRange(1, 50)) // true (checks length)
    WriteLine("Valid name length");
```

### DateTime Extensions

#### Rounding Methods
Round DateTime and DateTimeOffset to nearest intervals:
- **`Round()`** - Rounds to nearest interval (midpoint rounds up)
- **`RoundUp()`** - Always rounds up to next interval
- **`RoundDown()`** - Always rounds down to previous interval

Preserves `DateTime.Kind` and `DateTimeOffset.Offset`. Uses integer arithmetic for precise midpoint handling.

```csharp
DateTime dt = new DateTime(2024, 1, 15, 10, 37, 0);

// Round to nearest 15 minutes
DateTime rounded = dt.Round(TimeSpan.FromMinutes(15)); // 10:30:00

// Round up to next hour
DateTime roundedUp = dt.RoundUp(TimeSpan.FromHours(1)); // 11:00:00

// Round down to previous hour
DateTime roundedDown = dt.RoundDown(TimeSpan.FromHours(1)); // 10:00:00

// Works with DateTimeOffset too
DateTimeOffset dto = DateTimeOffset.Now;
DateTimeOffset rounded2 = dto.Round(TimeSpan.FromMinutes(5));
```

### DateTimeOffset Offset Conversions

Extensions for converting `DateTimeOffset` and `DateTime` values between different timezone offsets. These methods fill framework gaps and bypass `DateTime.Kind` restrictions that often cause issues with deserialized or external timestamps.

#### ToOffsetSameUtc - Change Offset, Preserve UTC Instant

**`ToOffsetSameUtc()`** - 🌟 Changes the offset while keeping the same UTC instant 🌟

```csharp
// Start with 3:00 PM +05:00 (UTC: 10:00 AM)
var dt = new DateTimeOffset(2024, 1, 15, 15, 0, 0, TimeSpan.FromHours(5));

// Change to Pacific time offset (-08:00), UTC stays 10:00 AM
var result = dt.ToOffsetSameUtc(TimeSpan.FromHours(-8));
// Result: 2024-01-15 02:00 AM -08:00 (UTC still 10:00 AM)

// Works with TimeZoneInfo too
var estTime = dt.ToOffsetSameUtc(TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
// Result: 2024-01-15 05:00 AM -05:00 (UTC still 10:00 AM)
```

**Note:** Framework's `DateTimeOffset.ToOffset()` behaves the same way - it preserves UTC time. `ToOffsetSameUtc` provides a `TimeZoneInfo` overload that the framework lacks.

#### DateTime to DateTimeOffset Conversions

Convert `DateTime` to `DateTimeOffset` with explicit control over interpretation. **Importantly, these methods ignore `DateTime.Kind`**, bypassing framework limitations that throw exceptions when `Kind` doesn't match expectations.

**`ToDateTimeOffset()`** - Treats DateTime as local time, preserves ticks

```csharp
// DateTime represents 2:00 PM in some timezone
var dt = new DateTime(2024, 1, 15, 14, 0, 0);

// Create DateTimeOffset treating it as Pacific time
var result = dt.ToDateTimeOffset(TimeSpan.FromHours(-8));
// Result: 2024-01-15 2:00 PM -08:00 (UTC: 10:00 PM)
```

**`ToDateTimeOffsetFromUtc()`** - Treats DateTime as UTC, adjusts local time by offset

```csharp
// DateTime represents 10:00 AM UTC (from database, API, etc.)
var utcTime = new DateTime(2024, 1, 15, 10, 0, 0);

// Convert to Eastern time representation
var result = utcTime.ToDateTimeOffsetFromUtc(TimeSpan.FromHours(-5));
// Result: 2024-01-15 5:00 AM -05:00 (UTC: 10:00 AM)

// Works with TimeZoneInfo
var est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
var result2 = utcTime.ToDateTimeOffsetFromUtc(est);
// Result: 2024-01-15 5:00 AM -05:00
```

### Conditional LINQ Extensions

Methods for conditionally applying LINQ operations, perfect for building dynamic queries and avoiding if-else chains.

- **`WhereIf()`** - 🌟 Applies Where filter only if condition is true 🌟
- **`WhereIfElse()`** - Applies one of two Where filters based on condition
- **`SkipIf()`** / **`TakeIf()`** - Conditionally skip or take elements
- **`SkipTakeIf()`** - Conditionally skip and take elements (pagination)

```csharp
// Build dynamic query without if-else chains
var query = users
    .WhereIf(filterByAge, u => u.Age >= 18)
    .WhereIf(filterByActive, u => u.IsActive)
    .TakeIf(limitResults, 100);

// Conditional pagination
var results = items.SkipTakeIf(paginationEnabled, skip: 20, count: 10);
```

### Null-Safe Collection and Value Utilities

Extensions for graceful handling of null collections and nullable values.

- **`EmptyIfNull`** - 🌟 Returns empty collection/array/string if null (property) 🌟
- **`IsDefault`** / **`NotDefault`** - Check if a struct value equals its default; null-safe on `Nullable<T>`
- **`NullIfDefault`** - Returns null if a struct value equals its default; null-safe on `Nullable<T>`
- **`ValueOrDefault`** - Gets the value if not null, else `default(T)` (property, on `Nullable<T>`)
- **`ValueOr(alt)`** - Returns `alt` if null **or default** (treats both as "not set"), otherwise the value

```csharp
// Avoid null reference exceptions in chains
int[] nullArray = null;
int count = nullArray.EmptyIfNull.Length; // 0 (no exception)

string nullStr = null;
string safe = nullStr.EmptyIfNull; // ""

// Nullable<T> value helpers
int? val = null;
int a = val.ValueOrDefault;  // 0 (property)
int b = val.ValueOr(42);     // 42

int? zero = 0;
int c = zero.ValueOr(42);    // 42 (0 == default, treated as "not set")

int? five = 5;
int d = five.ValueOr(42);    // 5

// NullIfDefault on struct
Guid empty = Guid.Empty;
Guid? result = empty.NullIfDefault; // null
```

### Dictionary Extensions

#### Comparison
- **`DictionariesAreEqual()`** - Deep equality comparison of dictionaries

```csharp
var dict1 = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
var dict2 = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };

bool equal = dict1.DictionariesAreEqual(dict2); // true

// With custom comparer
var dict3 = new Dictionary<string, User> { ["alice"] = new User("Alice", 25) };
var dict4 = new Dictionary<string, User> { ["alice"] = new User("Alice", 25) };
bool equal2 = dict3.DictionariesAreEqual(dict4, (u1, u2) => u1.Name == u2.Name);
```

### Path Utilities (PathX)

Clean path manipulation that handles nulls gracefully and normalizes paths. All paths are automatically converted to use forward slashes (`/`) instead of backslashes for cross-platform consistency. 🌟

- 🌟🌟 **All paths are automatically converted to use forward slashes (`/`) instead of backslashes.** 🌟🌟 (throughout all the following)
- **`CleanPath()`** - Normalizes path to use forward slashes and trims
- **`GetFullPath()`** - Gets full path with null handling and cleaning
- **`GetDirectoryName()`** - Gets directory name with null handling and cleaning
- **`PathCombine()`** - Combines paths with cleaning


```csharp
string path = PathX.CleanPath(@"C:\Users\Documents\");
// Result: "C:/Users/Documents"

string fullPath = PathX.GetFullPath("./file.txt");
// Gets full path and cleans it

string combined = PathX.PathCombine("folder", "subfolder/file.txt");
// Result: "folder/subfolder/file.txt"
```

### Console Output

- **`Print()`** - 🌟🌟 Quick console output for strings and objects 🌟🌟

```csharp
"Hello World".Print(); // Outputs to console and returns the string

var obj = new { Name = "Test", Value = 42 };
obj.Print(); // Outputs object to console
```

## Performance Features

Many methods in this library are optimized for performance:
- `[MethodImpl(MethodImplOptions.AggressiveInlining)]` for hot-path methods. This is key, especially for the string and character methods that are called frequently.
- ASCII character methods use direct numeric comparisons instead of slower .NET methods
- `TrimIfNeeded()` checks before allocating — returns the same string reference when no trim is required
- Line processing methods use efficient span-based operations and deferred execution where appropriate

## Target Frameworks

- .NET 10.0 (C# 14 required for extension properties)

## License

MIT

## Relationship to DotNetXtensions

This is an updated, .NET Core only version of the older, but currently more comprehensive [DotNetXtensions](https://github.com/copernicus365/DotNetXtensions) library. Yet the gap is decreasing, as I add back core extensions. But I'm improving them as we go, and I fully intend to keep this a leaner and meaner DotNetXtensions, filtering out many things that were more niche, or bad decisions, etc.

Issues and pull requests are welcome at the [GitHub repository](https://github.com/copernicus365/DotNetXtensions.Mini).
