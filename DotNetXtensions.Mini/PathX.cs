namespace DotNetXtensions;

/// <summary>Cleans up paths to use forward slashes, handles nulls gracefully, usually trims.</summary>
public static class PathX
{
	/// <summary>Calls `Path.GetFullPath`, but handles nulls, and cleans the path with `CleanPath`.</summary>
	public static string GetFullPath(string path)
		=> CleanPath(Path.GetFullPath(path ?? ""));

	/// <summary>Null-safe wrapper around <see cref="Path.GetDirectoryName(string)"/> that normalizes the result via <see cref="CleanPath"/>.</summary>
	public static string GetDirectoryName(string path)
		=> CleanPath(Path.GetDirectoryName(path ?? ""));

	/// <summary>Normalizes a path by replacing backslashes with forward slashes and trimming whitespace; returns an empty string for null input.</summary>
	public static string CleanPath(string path)
		=> (path ?? "")?.TrimIfNeeded()?.Replace('\\', '/');

	/// <summary>Combines two paths via <see cref="Path.Combine(string, string)"/> and normalizes the result via <see cref="CleanPath"/>; when <paramref name="trim"/> is true, trims each path before combining.</summary>
	public static string PathCombine(string path1, string path2, bool trim = false)
		=> CleanPath(trim ? Path.Combine(path1.TrimIfNeeded(), path2.TrimIfNeeded()) : Path.Combine(path1, path2));
}	