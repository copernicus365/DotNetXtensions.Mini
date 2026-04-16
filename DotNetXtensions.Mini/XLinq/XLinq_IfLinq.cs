using System.Linq.Expressions;

namespace DotNetXtensions;

public static partial class XLinq
{
	// --- WhereIf ---

	/// <summary>Applies <paramref name="predicate"/> as a Where filter when <paramref name="condition"/> is true; returns <paramref name="source"/> unchanged otherwise. Null-safe.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
		=> !condition ? source : source?.Where(predicate);

	/// <inheritdoc cref="WhereIf{T}(IEnumerable{T}, bool, Func{T, bool})"/>
	[DebuggerStepThrough]
	public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicate)
		=> !condition ? source : source?.Where(predicate);

	/// <inheritdoc cref="WhereIf{T}(IEnumerable{T}, bool, Func{T, bool})"/>
	[DebuggerStepThrough]
	public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
		=> !condition ? source : source?.Where(predicate);

	/// <inheritdoc cref="WhereIf{T}(IEnumerable{T}, bool, Func{T, bool})"/>
	[DebuggerStepThrough]
	public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, int, bool>> predicate)
		=> !condition ? source : source?.Where(predicate);


	// --- WhereIfElse ---

	/// <summary>Applies <paramref name="predicateIf"/> when <paramref name="condition"/> is true, <paramref name="predicateElse"/> otherwise. Null-safe.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> WhereIfElse<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicateIf, Func<T, bool> predicateElse)
		=> condition ? source?.Where(predicateIf) : source?.Where(predicateElse);

	/// <inheritdoc cref="WhereIfElse{T}(IEnumerable{T}, bool, Func{T, bool}, Func{T, bool})"/>
	[DebuggerStepThrough]
	public static IEnumerable<T> WhereIfElse<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicateIf, Func<T, int, bool> predicateElse)
		=> condition ? source?.Where(predicateIf) : source?.Where(predicateElse);

	/// <inheritdoc cref="WhereIfElse{T}(IEnumerable{T}, bool, Func{T, bool}, Func{T, bool})"/>
	[DebuggerStepThrough]
	public static IQueryable<T> WhereIfElse<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicateIf, Expression<Func<T, bool>> predicateElse)
		=> condition ? source?.Where(predicateIf) : source?.Where(predicateElse);

	/// <inheritdoc cref="WhereIfElse{T}(IEnumerable{T}, bool, Func{T, bool}, Func{T, bool})"/>
	[DebuggerStepThrough]
	public static IQueryable<T> WhereIfElse<T>(this IQueryable<T> source, bool condition, Expression<Func<T, int, bool>> predicateIf, Expression<Func<T, int, bool>> predicateElse)
		=> condition ? source?.Where(predicateIf) : source?.Where(predicateElse);


	// --- SkipIf ---

	/// <summary>Skips <paramref name="count"/> elements when <paramref name="condition"/> is true; returns <paramref name="source"/> unchanged otherwise. Null-safe.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> SkipIf<T>(this IEnumerable<T> source, bool condition, int count)
		=> !condition ? source : source?.Skip(count);

	/// <inheritdoc cref="SkipIf{T}(IEnumerable{T}, bool, int)"/>
	[DebuggerStepThrough]
	public static IQueryable<T> SkipIf<T>(this IQueryable<T> source, bool condition, int count)
		=> !condition ? source : source?.Skip(count);


	// --- TakeIf ---

	/// <summary>Takes <paramref name="count"/> elements when <paramref name="condition"/> is true; returns <paramref name="source"/> unchanged otherwise. Null-safe.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> TakeIf<T>(this IEnumerable<T> source, bool condition, int count)
		=> !condition ? source : source?.Take(count);

	/// <inheritdoc cref="TakeIf{T}(IEnumerable{T}, bool, int)"/>
	[DebuggerStepThrough]
	public static IQueryable<T> TakeIf<T>(this IQueryable<T> source, bool condition, int count)
		=> !condition ? source : source?.Take(count);


	// --- SkipTakeIf ---

	/// <summary>Skips <paramref name="skip"/> then takes <paramref name="count"/> elements when <paramref name="condition"/> is true; returns <paramref name="source"/> unchanged otherwise. Null-safe.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> SkipTakeIf<T>(this IEnumerable<T> source, bool condition, int skip, int count)
		=> !condition ? source : source?.Skip(skip).Take(count);

	/// <inheritdoc cref="SkipTakeIf{T}(IEnumerable{T}, bool, int, int)"/>
	[DebuggerStepThrough]
	public static IQueryable<T> SkipTakeIf<T>(this IQueryable<T> source, bool condition, int skip, int count)
		=> !condition ? source : source?.Skip(skip).Take(count);
}
