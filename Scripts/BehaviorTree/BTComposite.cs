using Godot;
using System.Collections.Generic;

/// <summary>
/// Base class for composite nodes (nodes with children)
/// Handles child management and initialization
/// </summary>
public abstract partial class BTComposite : BTNode
{
	protected List<BTNode> children = new List<BTNode>();

	/// <summary>
	/// Add a child node to this composite
	/// </summary>
	public void AddChild(BTNode child)
	{
		children.Add(child);
	}

	/// <summary>
	/// Initialize this node and all children
	/// </summary>
	public override void Initialize(Node2D agent, Blackboard blackboard)
	{
		base.Initialize(agent, blackboard);
		
		foreach (var child in children)
		{
			child.Initialize(agent, blackboard);
		}
	}

	/// <summary>
	/// Reset this node and all children
	/// </summary>
	public override void Reset()
	{
		base.Reset();
		
		foreach (var child in children)
		{
			child.Reset();
		}
	}
}
