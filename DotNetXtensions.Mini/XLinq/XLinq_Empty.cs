namespace DotNetXtensions;

public static partial class XLinq
{
	extension(string s)
	{
		/// <summary>Returns an empty string if null, else returns the string.</summary>
		public string EmptyIfNull => s ?? "";

		/// <summary>Returns null if empty string, else returns the string.</summary>
		public string NullIfEmpty => s == "" ? null : s;

		/// <summary>Returns null if empty string, else returns the string.</summary>
		public string NullIfWhitespace => string.IsNullOrWhiteSpace(s) ? null : s;

		/// <summary>Returns the length, or 0 if null.</summary>
		public int LengthN => s == null ? 0 : s.Length;

		/// <summary>Returns true if null or empty.</summary>
		public bool IsEmpty => s == null || s.Length == 0;

		/// <summary>Returns true if not null and not empty.</summary>
		public bool NotEmpty => s != null && s.Length != 0;

		/// <summary>Returns true if null or whitespace.</summary>
		public bool IsEmptyOrWhiteSpace => string.IsNullOrWhiteSpace(s);

		/// <summary>Returns the first non-null/empty string among the receiver and up to two additional inputs. Returns null if all are null or empty.</summary>
		[DebuggerStepThrough]
		public string FirstNotNullOrEmpty(string value2, string value3 = null)
		{
			if(s != null && s.Length != 0) return s;
			if(value2 != null && value2.Length != 0) return value2;
			if(value3 != null && value3.Length != 0) return value3;
			return null;
		}
	}

	extension<T>(T[] arr)
	{
		/// <summary>Returns an empty array if null, else returns the array.</summary>
		public T[] EmptyIfNull => arr ?? [];

		/// <summary>Returns the length, or 0 if null.</summary>
		public int LengthN => arr == null ? 0 : arr.Length;

		/// <summary>Returns true if null or empty.</summary>
		public bool IsEmpty => arr == null || arr.Length < 1;

		/// <summary>Returns true if null or empty.</summary>
		public bool IsNullOrEmpty => arr == null || arr.Length < 1;

		/// <summary>Returns true if not null and not empty.</summary>
		public bool NotEmpty => arr != null && arr.Length > 0;

		/// <summary>Returns true if not null and not empty.</summary>
		public bool NotNullOrEmpty => arr != null && arr.Length > 0;
	}

	extension<T>(IEnumerable<T> enumerable)
	{
		/// <summary>Returns an empty collection if null, else returns the collection.</summary>
		public IEnumerable<T> EmptyIfNull => enumerable ?? [];
	}

	// NullIfDefault

	extension<T>(T t) where T : struct
	{
		/// <summary>Returns null if the value equals default, else returns the original value.</summary>
		public T? NullIfDefault => EqualityComparer<T>.Default.Equals(t, default) ? null : t;

		/// <summary>Returns the specified value if the value equals default, else returns the original value.</summary>
		public T ValueIfDefault(T value) => EqualityComparer<T>.Default.Equals(t, default) ? value : t;
	}

	// CountN

	extension<T>(IList<T> list)
	{
		/// <summary>Returns the count, or 0 if null.</summary>
		public int CountN => list == null ? 0 : list.Count;
	}

	extension<T>(ICollection<T> coll)
	{
		/// <summary>Returns the count, or 0 if null.</summary>
		public int CountN => coll == null ? 0 : coll.Count;

		/// <summary>Returns true if null or empty.</summary>
		public bool IsEmpty => coll == null || coll.Count < 1;

		/// <summary>Returns true if null or empty.</summary>
		public bool IsNullOrEmpty => coll == null || coll.Count < 1;

		/// <summary>Returns true if not null and not empty.</summary>
		public bool NotEmpty => coll != null && coll.Count > 0;

		/// <summary>Returns true if not null and not empty.</summary>
		public bool NotNullOrEmpty => coll != null && coll.Count > 0;
	}

	extension<TValue>(TValue? value) where TValue : struct
	{
		/// <summary>Returns true if null or equals the default value.</summary>
		public bool IsDefault => value == null || EqualityComparer<TValue>.Default.Equals(value.Value, default);

		/// <summary>Returns true if null or equals the default value.</summary>
		public bool IsNullOrDefault => value == null || EqualityComparer<TValue>.Default.Equals(value.Value, default);

		/// <summary>Returns true if not null and not equal to the default value.</summary>
		public bool NotDefault => value != null && !EqualityComparer<TValue>.Default.Equals(value.Value, default);

		/// <summary>Returns true if not null and not equal to the default value.</summary>
		public bool NotNullOrDefault => value != null && !EqualityComparer<TValue>.Default.Equals(value.Value, default);

		/// <summary>Returns the value if not null, else default if null.</summary>
		public TValue ValueOrDefault => value ?? default;

		/// <summary>Returns the value if it is set (not null and not default), else returns input 'or' value.</summary>
		public TValue ValueOr(TValue alt) => value == null || EqualityComparer<TValue>.Default.Equals(value.Value, default) ? alt : value.Value;
	}

	extension<T>(T t) where T : class, new()
	{
		/// <summary>Returns a new instance if null, else returns the object.</summary>
		public T EmptyIfNull => t ?? new T();
	}
}
