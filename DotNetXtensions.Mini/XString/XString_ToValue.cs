namespace DotNetXtensions;

public static partial class XString
{
	/// <summary>Parses the string as an int; returns <paramref name="dflt"/> if parsing fails.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ToInt(this string val, int dflt = 0)
		=> int.TryParse(val, out int v) ? v : dflt;

	/// <summary>Parses the string as a long; returns <paramref name="dflt"/> if parsing fails.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long ToLong(this string val, long dflt = 0)
		=> long.TryParse(val, out long v) ? v : dflt;

	/// <summary>Parses the string as a decimal; returns <paramref name="dflt"/> if parsing fails.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static decimal ToDecimal(this string val, decimal dflt = 0)
		=> decimal.TryParse(val, out decimal v) ? v : dflt;

	/// <summary>Parses the string as a double; returns <paramref name="dflt"/> if parsing fails.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double ToDouble(this string val, double dflt = 0)
		=> double.TryParse(val, out double v) ? v : dflt;

	/// <summary>Parses the string as a bool; also accepts "0" (false) and "1" (true). Returns <paramref name="dflt"/> if parsing fails.</summary>
	public static bool ToBool(this string val, bool dflt = false)
	{
		if(val.NotEmpty) {
			if(bool.TryParse(val, out bool i))
				return i;

			if(int.TryParse(val, out int num) && num < 2) {
				if(num == 0) return false;
				if(num == 1) return true;
			}
		}
		return dflt;
	}

	/// <summary>
	/// Parses the string as a <see cref="DateTime"/> via <see cref="DateTimeOffset.TryParse(string, out DateTimeOffset)"/>;
	/// returns <paramref name="defaultVal"/> or <see cref="DateTime.MinValue"/> on failure.
	/// </summary>
	public static DateTime ToDateTime(this string val, DateTime? defaultVal = null)
	{
		if(val.NotEmpty) {
			// NOTE!!  parses DateTimeOffset and if true gets the DateTime...
			// big issues with frameword using computer's local time in these considerations,
			// this was probably not wise though :think: ... but change now would be breaking...
			if(DateTimeOffset.TryParse(val, out DateTimeOffset dt))
				return dt.DateTime;
		}
		return defaultVal ?? DateTime.MinValue;
	}

	/// <summary>Parses the string as a <see cref="DateTimeOffset"/>; returns <paramref name="dflt"/> or <see cref="DateTimeOffset.MinValue"/> on failure.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTimeOffset ToDateTimeOffset(this string val, DateTimeOffset? dflt = null)
		=> DateTimeOffset.TryParse(val, out DateTimeOffset v) ? v : (dflt ?? DateTimeOffset.MinValue);

	/// <summary>Parses the string as a <see cref="Guid"/>; returns <paramref name="dflt"/> or <see langword="default"/> on failure.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Guid ToGuid(this string val, Guid? dflt = null)
		=> Guid.TryParse(val, out Guid v) ? v : (dflt ?? default);



	/// <summary>Parses the string as a nullable int; returns <see langword="null"/> if empty or parsing fails.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int? ToIntN(this string val)
		=> val.NotEmpty && int.TryParse(val, out int v) ? v : null;

	/// <summary>Parses the string as a nullable long; returns <see langword="null"/> if empty or parsing fails.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long? ToLongN(this string val)
		=> val.NotEmpty && long.TryParse(val, out long v) ? v : null;

	/// <summary>Parses the string as a nullable decimal; returns <see langword="null"/> if empty or parsing fails.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static decimal? ToDecimalN(this string val)
		=> val.NotEmpty && decimal.TryParse(val, out decimal v) ? v : null;

	/// <summary>Parses the string as a nullable double; returns <see langword="null"/> if empty or parsing fails.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double? ToDoubleN(this string val)
		=> val.NotEmpty && double.TryParse(val, out double v) ? v : null;

	/// <summary>Parses the string as a nullable bool (also accepts "0"/"1"); returns <see langword="null"/> if empty.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool? ToBoolN(this string val)
		=> val.IsEmpty ? null : ToBool(val); // must use ToBool, handles numeric...

	/// <summary>Parses the string as a nullable <see cref="DateTime"/> (via <see cref="DateTimeOffset.TryParse(string, out DateTimeOffset)"/>); returns <see langword="null"/> if empty or parsing fails.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTime? ToDateTimeN(this string val)
		=> val.NotEmpty && DateTimeOffset.TryParse(val, out DateTimeOffset v) // see notes above: ToDateTime()
		? v.DateTime : null;

	/// <summary>Parses the string as a nullable <see cref="DateTimeOffset"/>; returns <see langword="null"/> if empty or parsing fails.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTimeOffset? ToDateTimeOffsetN(this string val)
		=> val.NotEmpty && DateTimeOffset.TryParse(val, out DateTimeOffset v) ? v : null;

	/// <summary>Parses the string as a nullable <see cref="Guid"/>; returns <see langword="null"/> if empty or parsing fails.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Guid? ToGuidN(this string val)
		=> val.NotEmpty && Guid.TryParse(val, out Guid v) ? v : null;
}
