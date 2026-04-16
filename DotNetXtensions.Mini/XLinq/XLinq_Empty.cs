namespace DotNetXtensions;

/// <summary>
/// Extension methods for strings, arrays, collections, and generic types — null-safe empty/default checks,
/// null-coalescing helpers, and conditional LINQ operators.
/// </summary>
public static partial class XLinq
{
	// --- string ---

	extension(string s)
	{
		/// <summary>Returns an empty string if null, else returns the string.</summary>
		public string EmptyIfNull => s ?? "";

		/// <summary>Returns null if empty string, else returns the string.</summary>
		public string NullIfEmpty => s == "" ? null : s;

		/// <summary>Returns null if value is null, empty, or consists exclusively of white-space. Else returns the string.</summary>
		public string NullIfWhitespace => string.IsNullOrWhiteSpace(s) ? null : s;

		/// <summary>Returns the length, or 0 if null.</summary>
		public int LengthN => s == null ? 0 : s.Length;

		/// <summary>Returns true if null or empty. Null-safe.</summary>
		/// <remarks>Extension-property form of the static <c>string.IsNullOrEmpty</c>.</remarks>
		public bool IsEmpty => s == null || s.Length == 0;

		/// <summary>Returns true if not null and not empty. Null-safe.</summary>
		/// <remarks>Extension-property form of <c>!string.IsNullOrEmpty</c>.</remarks>
		public bool NotEmpty => s != null && s.Length != 0;

		/// <summary>Returns true if value is null, empty, or consists exclusively of white-space. Null-safe. This is an indirection call to <c>string.IsNullOrWhiteSpace</c></summary>
		/// <remarks>Extension-property form of the static <c>string.IsNullOrWhiteSpace</c>. The inverted prefix avoids a name conflict with that static method.</remarks>
		public bool IsEmptyOrWhiteSpace => string.IsNullOrWhiteSpace(s);

		/// <summary>Returns the first non-null/empty string, checking up to two additional values. Returns null if all are null or empty.</summary>
		[DebuggerStepThrough]
		public string FirstNotNullOrEmpty(string value2, string value3 = null)
		{
			if(s != null && s.Length != 0) return s;
			if(value2 != null && value2.Length != 0) return value2;
			if(value3 != null && value3.Length != 0) return value3;
			return null;
		}
	}

	// --- array ---

	extension<T>(T[] arr)
	{
		/// <summary>Returns an empty array if null, else returns the array.</summary>
		public T[] EmptyIfNull => arr ?? [];

		/// <summary>Returns the length, or 0 if null.</summary>
		public int LengthN => arr == null ? 0 : arr.Length;

		/// <summary>Returns true if null or empty. Null-safe.</summary>
		/// <remarks>Short form of <c>IsNullOrEmpty</c>.</remarks>
		public bool IsEmpty => arr == null || arr.Length < 1;

		/// <summary>Returns true if null or empty.</summary>
		/// <remarks>Full-name alias for <c>IsEmpty</c>.</remarks>
		public bool IsNullOrEmpty => arr == null || arr.Length < 1;

		/// <summary>Returns true if not null and not empty. Null-safe.</summary>
		/// <remarks>Short form of <c>NotNullOrEmpty</c>.</remarks>
		public bool NotEmpty => arr != null && arr.Length > 0;

		/// <summary>Returns true if not null and not empty.</summary>
		/// <remarks>Full-name alias for <c>NotEmpty</c>.</remarks>
		public bool NotNullOrEmpty => arr != null && arr.Length > 0;
	}

	// --- enumerable ---

	extension<T>(IEnumerable<T> enumerable)
	{
		/// <summary>Returns an empty collection if null, else returns the collection.</summary>
		public IEnumerable<T> EmptyIfNull => enumerable ?? [];
	}

	// --- IList ---

	extension<T>(IList<T> list)
	{
		/// <summary>Returns the count, or 0 if null.</summary>
		public int CountN => list == null ? 0 : list.Count;
	}

	// --- ICollection ---

	extension<T>(ICollection<T> coll)
	{
		/// <summary>Returns the count, or 0 if null.</summary>
		public int CountN => coll == null ? 0 : coll.Count;

		/// <summary>Returns true if null or empty. Null-safe.</summary>
		/// <remarks>Short form of <c>IsNullOrEmpty</c>.</remarks>
		public bool IsEmpty => coll == null || coll.Count < 1;

		/// <summary>Returns true if null or empty.</summary>
		/// <remarks>Full-name alias for <c>IsEmpty</c>.</remarks>
		public bool IsNullOrEmpty => coll == null || coll.Count < 1;

		/// <summary>Returns true if not null and not empty. Null-safe.</summary>
		/// <remarks>Short form of <c>NotNullOrEmpty</c>.</remarks>
		public bool NotEmpty => coll != null && coll.Count > 0;

		/// <summary>Returns true if not null and not empty.</summary>
		/// <remarks>Full-name alias for <c>NotEmpty</c>.</remarks>
		public bool NotNullOrEmpty => coll != null && coll.Count > 0;
	}

	// --- new() class ---

	extension<T>(T t) where T : class, new()
	{
		/// <summary>Returns a new instance if null, else returns the object.</summary>
		public T EmptyIfNull => t ?? new T();
	}

	// --- struct ---

	extension<T>(T t) where T : struct
	{
		/// <summary>Returns true if the value equals default.</summary>
		public bool IsDefault => EqualityComparer<T>.Default.Equals(t, default);

		/// <summary>Returns true if the value does not equal default.</summary>
		public bool NotDefault => !EqualityComparer<T>.Default.Equals(t, default);

		/// <summary>Returns null if the value equals default, else returns the original value.</summary>
		public T? NullIfDefault => EqualityComparer<T>.Default.Equals(t, default) ? null : t;

		/// <summary>Returns value if not default, otherwise returns <c>alt</c>.</summary>
		public T ValueOr(T alt) => EqualityComparer<T>.Default.Equals(t, default) ? alt : t;
	}

	// --- nullable struct ---

	extension<T>(T? value) where T : struct
	{
		/// <summary>Returns true if null or equals the default value. Null-safe.</summary>
		/// <remarks>Short form of <c>IsNullOrDefault</c>.</remarks>
		public bool IsDefault => value == null || EqualityComparer<T>.Default.Equals(value.Value, default);

		/// <summary>Returns true if null or equals the default value.</summary>
		/// <remarks>Full-name alias for <c>IsDefault</c>.</remarks>
		public bool IsNullOrDefault => value == null || EqualityComparer<T>.Default.Equals(value.Value, default);

		/// <summary>Returns true if not null and not equal to the default value. Null-safe.</summary>
		/// <remarks>Short form of <c>NotNullOrDefault</c>.</remarks>
		public bool NotDefault => value != null && !EqualityComparer<T>.Default.Equals(value.Value, default);

		/// <summary>Returns true if not null and not equal to the default value.</summary>
		/// <remarks>Full-name alias for <c>NotDefault</c>.</remarks>
		public bool NotNullOrDefault => value != null && !EqualityComparer<T>.Default.Equals(value.Value, default);

		/// <summary>Returns null if null or equals the default value, else returns the value.</summary>
		public T? NullIfDefault => (value == null || EqualityComparer<T>.Default.Equals(value.Value, default)) ? null : value;

		/// <summary>Returns the value if not null, otherwise returns default.</summary>
		/// <remarks>Extension-property equivalent of the framework's <c>Nullable{T}.GetValueOrDefault()</c>.</remarks>
		public T ValueOrDefault => value ?? default;

		/// <summary>Returns value if not null or default, otherwise returns <c>alt</c>.</summary>
		public T ValueOr(T alt) => value == null || EqualityComparer<T>.Default.Equals(value.Value, default) ? alt : value.Value;
	}
}
