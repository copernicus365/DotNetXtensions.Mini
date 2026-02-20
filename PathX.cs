namespace DotNetXtensions;

// TO INCLUDE or NOT to include... that is the question

/// <summary>Cleans up paths to use forward slashes, handles nulls gracefully, usually trims.</summary>
public static class PathX
{
	/// <summary>Calls `Path.GetFullPath`, but handles nulls, and cleans the path with `CleanPath`.</summary>
	public static string GetFullPath(string path)
		=> CleanPath(Path.GetFullPath(path ?? ""));

	public static string GetDirectoryName(string path)
		=> CleanPath(Path.GetDirectoryName(path ?? ""));

	public static string CleanPath(string path)
		=> (path ?? "")?.TrimIfNeeded()?.Replace('\\', '/');

	public static string PathCombine(string path1, string path2, bool trim = false)
		=> CleanPath(trim ? Path.Combine(path1.TrimIfNeeded(), path2.TrimIfNeeded()) : Path.Combine(path1, path2));
}	