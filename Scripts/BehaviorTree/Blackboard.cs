using Godot;
using System.Collections.Generic;

// From Starter

/// <summary>
/// Blackboard pattern: Shared memory for behavior tree
/// Stores data that multiple nodes need to access
/// </summary>
public partial class Blackboard : Node
{
	private Dictionary<string, object> data = new Dictionary<string, object>();

	/// <summary>
	/// Store a value in the blackboard
	/// </summary>
	public void SetValue(string key, object value)
	{
		if (data.ContainsKey(key))
			data[key] = value;
		else
			data.Add(key, value);
	}

	/// <summary>
	/// Retrieve a value from the blackboard
	/// </summary>
	public T GetValue<T>(string key)
	{
		if (data.ContainsKey(key))
			return (T)data[key];
		return default(T);
	}

	/// <summary>
	/// Check if a key exists in the blackboard
	/// </summary>
	public bool HasValue(string key)
	{
		return data.ContainsKey(key);
	}

	/// <summary>
	/// Remove a value from the blackboard
	/// </summary>
	public void RemoveValue(string key)
	{
		if (data.ContainsKey(key))
			data.Remove(key);
	}

	/// <summary>
	/// Clear all data
	/// </summary>
	public void Clear()
	{
		data.Clear();
	}
}
