namespace DNX.Test;

public static class TestsX
{
	public static void IsEmpty<T>(ICollection<T> c)
		=> Equal(0, c.Count);

	public static void IsSingle<T>(ICollection<T> c)
		=> Equal(1, c.Count);

	public static void IsCount<T>(int expectedCount, ICollection<T> c)
		=> Equal(expectedCount, c.Count);
}
