using System.Linq.Expressions;

namespace DotNetXtensions;

public static partial class XLinq
{
	// --- WhereIf ---

	/// <summary>Applies Where filter if condition is true.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
		=> !condition ? source : source?.Where(predicate);

	/// <summary>Applies indexed Where filter if condition is true.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicate)
		=> !condition ? source : source?.Where(predicate);

	/// <summary>Applies Where filter if condition is true.</summary>
	[DebuggerStepThrough]
	public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
		=> !condition ? source : source?.Where(predicate);

	/// <summary>Applies indexed Where filter if condition is true.</summary>
	[DebuggerStepThrough]
	public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, int, bool>> predicate)
		=> !condition ? source : source?.Where(predicate);


	// --- WhereIfElse ---

	/// <summary>Applies one of two Where filters based on condition.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> WhereIfElse<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicateIf, Func<T, bool> predicateElse)
		=> condition ? source?.Where(predicateIf) : source?.Where(predicateElse);

	/// <summary>Applies one of two indexed Where filters based on condition.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> WhereIfElse<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicateIf, Func<T, int, bool> predicateElse)
		=> condition ? source?.Where(predicateIf) : source?.Where(predicateElse);

	/// <summary>Applies one of two Where filters based on condition.</summary>
	[DebuggerStepThrough]
	public static IQueryable<T> WhereIfElse<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicateIf, Expression<Func<T, bool>> predicateElse)
		=> condition ? source?.Where(predicateIf) : source?.Where(predicateElse);

	/// <summary>Applies one of two indexed Where filters based on condition.</summary>
	[DebuggerStepThrough]
	public static IQueryable<T> WhereIfElse<T>(this IQueryable<T> source, bool condition, Expression<Func<T, int, bool>> predicateIf, Expression<Func<T, int, bool>> predicateElse)
		=> condition ? source?.Where(predicateIf) : source?.Where(predicateElse);


	// --- SkipIf ---

	/// <summary>Skips elements if condition is true.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> SkipIf<T>(this IEnumerable<T> source, bool condition, int count)
		=> !condition ? source : source?.Skip(count);

	/// <summary>Skips elements if condition is true.</summary>
	[DebuggerStepThrough]
	public static IQueryable<T> SkipIf<T>(this IQueryable<T> source, bool condition, int count)
		=> !condition ? source : source?.Skip(count);


	// --- TakeIf ---

	/// <summary>Takes elements if condition is true.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> TakeIf<T>(this IEnumerable<T> source, bool condition, int count)
		=> !condition ? source : source?.Take(count);

	/// <summary>Takes elements if condition is true.</summary>
	[DebuggerStepThrough]
	public static IQueryable<T> TakeIf<T>(this IQueryable<T> source, bool condition, int count)
		=> !condition ? source : source?.Take(count);


	// --- SkipTakeIf ---

	/// <summary>Skips then takes elements if condition is true.</summary>
	[DebuggerStepThrough]
	public static IEnumerable<T> SkipTakeIf<T>(this IEnumerable<T> source, bool condition, int skip, int count)
		=> !condition ? source : source?.Skip(skip).Take(count);

	/// <summary>Skips then takes elements if condition is true.</summary>
	[DebuggerStepThrough]
	public static IQueryable<T> SkipTakeIf<T>(this IQueryable<T> source, bool condition, int skip, int count)
		=> !condition ? source : source?.Skip(skip).Take(count);
}
