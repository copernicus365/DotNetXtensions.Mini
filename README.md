# DotNetXtensions.Mini

[![NuGet](https://img.shields.io/nuget/v/DotNetXtensions.Mini.svg)](https://www.nuget.org/packages/DotNetXtensions.Mini)

A lightweight, streamlined version of [DotNetXtensions](https://github.com/copernicus365/DotNetXtensions) containing the most commonly used extension methods and utilities for .NET development. This mini version provides essential string, collection, character, date/time, and path manipulation utilities without the full library overhead. It is also possible to use the single file `DotNetXtensions.Mini.cs` directly in your project without needing to reference the NuGet package.

## Installation

Add the NuGet package to your project:

```bash
dotnet add package DotNetXtensions.Mini
```

## Use of `DnxMini.cs` single file

You can also choose to include the `DnxMini.cs` file directly in your project without referencing the NuGet package. This allows you to use the extension methods without adding an external dependency.

### A note on partial classes

For maintainability and for better testing, in some cases we internally have a number of separate partial classes (e.g. `XString_Nulle.cs`, `XString_ToValue.cs`, etc), even though they share the same class name: `partial class XString`. That said, note that when compiling, no artifacts of partial classes remain, and not that the nuget package is, of course, a compiled product of this source code, thus it has ZERO knowledge partials were ever used:

> When you use partial classes in C#, the C# compiler merges all the partial declarations into one single class during compilation. After compilation, there is no trace left of the partial keyword or the fact that the class was split across files — in the resulting assembly (IL / metadata), it looks exactly like you had written one big, normal (non-partial) class from the beginning. - Grok (2026-03)

## Features

### String Extensions

Unfortunately, EVEN in 2026 (!), .NET still lacks extension methods for extremely common string operations, the most famous of all: `IsNullOrEmpty`. Now perhaps the most controversial naming decision on my part is embracing in this almost singular case, an abbreviated name: `IsNulle()` and `NotNulle()`. If that bugs you, I hope you can forgive it! Feel free to use the non-abbreviated version: `IsNullOrEmpty()`.

#### Null/Empty Checks
- **`IsNulle()`** - 🌟 Checks if string is null or empty (faster than built-in methods) 🌟
- **`NotNulle()`** - 🌟 Checks if string is NOT null or empty 🌟
- **`IsNullOrEmpty()`** - 🌟 Alias for standard null/empty check 🌟
- **`IsNullOrWhiteSpace()`** - Extension wrapper for `string.IsNullOrWhiteSpace`
- **`NullIfEmpty()`** - 🌟 Returns null if string is empty, otherwise returns the string 🌟

```csharp
string str = "";
if (str.IsNulle()) // true
    WriteLine("Empty or null");

string name = "John";
if (name.NotNulle()) // true
    WriteLine($"Hello {name}");
```

#### String Trimming
- **`TrimIfNeeded()`** - Only trims if whitespace exists at start/end. More efficient, returns the same string when not.
- **`IsTrimmable()`** - Checks if string has leading/trailing whitespace
- **`TrimN()`** - Trims if not null, returns null otherwise.
- **`NullIfEmptyTrimmed()`** - 🌟 Trims and returns null if result is empty. This is another STAR that appears prolifically. 🌟

```csharp
string text = "  hello  ";
string trimmed = text.TrimIfNeeded(); // "hello"

string text2 = "hello";
bool needsTrim = text2.IsTrimmable(); // false (no trimming needed)
```

#### String Utilities
- **`ContainsIgnoreCase()`** - Case-insensitive contains check
- **`LengthN()`** / **`CountN()`** - Returns length or default value if null

```csharp
string str = "Hello World";
bool contains = str.ContainsIgnoreCase("WORLD"); // true

string nullStr = null;
int length = nullStr.LengthN(-1); // -1 (default value)
```

### Collection Extensions

#### Null/Empty Checks
- **`IsNulle()`** - 🌟🌟 Checks if collection/array is null or empty 🌟🌟
- **`NotNulle()`** - 🌟🌟 Checks if collection has items 🌟🌟

```csharp
List<int> numbers = new();
if (numbers.IsNulle()) // true
    WriteLine("No numbers");

int[] items = { 1, 2, 3 };
if (items.NotNulle()) // true
    WriteLine($"Has {items.Length} items");
```

#### Collection Utilities
- **`CountN()`** / **`LengthN()`** - Returns count/length or default value if null
- **`JoinToString()`** - Joins collection elements into string with separator 🌟

```csharp
List<int> numbers = null;
int count = numbers.CountN(); // 0

var names = new[] { "Alice", "Bob", "Charlie" };
string joined = names.JoinToString(); // "Alice,Bob,Charlie"

var users = new[] { new User("Alice"), new User("Bob") };
string userNames = users.JoinToString(u => u.Name, "; "); // "Alice; Bob"
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
if (name.InRange(1, 50)) // true (checks length)
    WriteLine("Valid name length");
```

### DateTime Extensions

#### Rounding Methods
Round DateTime and DateTimeOffset to nearest intervals:
- **`Round()`** - Rounds to nearest interval
- **`RoundUp()`** - Always rounds up to next interval
- **`RoundDown()`** - Always rounds down to previous interval
- **`RoundToNearest()`** - Rounds to nearest interval

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
DateTimeOffset rounded2 = dto.RoundToNearest(TimeSpan.FromMinutes(5));
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

*TO INCLUDE or NOT to include... that is the question*

Not sure if we will include this. I do however very frequently find myself needing this type of path manipulations.

Clean path manipulation that handles nulls gracefully and normalizes paths:
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
- `TrimIfNeeded()` checks before allocating new strings

## Target Framework

- .NET 8.0

## License

MIT

## Relationship to DotNetXtensions

This is a mini version of the older, but more comprehensive [DotNetXtensions](https://github.com/copernicus365/DotNetXtensions) library. I have found over the years, that for many projects, I only need a subset of the full library's features. Also that one maybe bloated on some parts, I'm striving to keep this lean. Still keeping this 'alpha' while we work out the best set of methods to include (early 2026)

## Contributing

Issues and pull requests are welcome at the [GitHub repository](https://github.com/copernicus365/DotNetXtensions.Mini).
