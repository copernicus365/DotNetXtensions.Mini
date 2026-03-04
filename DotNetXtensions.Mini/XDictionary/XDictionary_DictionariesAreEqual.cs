namespace DotNetXtensions;

public static partial class XDictionary
{
	public static bool DictionariesAreEqual<TKey, TValue>(
		this IDictionary<TKey, TValue> dict1,
		IDictionary<TKey, TValue> dict2,
		Func<TValue, TValue, bool> comparer = null)
	{
		if(dict1 == null || dict2 == null)
			return dict1 == null && dict2 == null;

		if(dict1.Count != dict2.Count)
			return false;

		bool hasEqCmpr = comparer != null;

		foreach(var kv in dict1) {
			TKey key1 = kv.Key;
			TValue val1 = kv.Value;

			if(!dict2.TryGetValue(key1, out TValue val2))
				return false;

			if(hasEqCmpr) {
				if(!comparer(val1, val2))
					return false;
			}
			else {
				if(!val1.Equals(val2))
					return false;
			}
		}
		return true;
	}
}
