using Godot;
using System.Collections.Generic;

/// <summary>
/// Base class for all behavior tree nodes
/// Defines the core interface and execution states
/// </summary>
public abstract partial class BTNode : Node
{
	// Execution states
	public enum Status
	{
		Success,  // Task completed successfully
		Failure,  // Task failed
		Running   // Task still in progress
	}

	// Reference to the AI agent (enemy)
	protected Node2D agent;
	
	// Reference to the blackboard (shared data)
	protected Blackboard blackboard;

	/// <summary>
	/// Initialize the node with agent and blackboard references
	/// </summary>
	public virtual void Initialize(Node2D agent, Blackboard blackboard)
	{
		this.agent = agent;
		this.blackboard = blackboard;
	}

	/// <summary>
	/// Main execution method - must be implemented by all nodes
	/// </summary>
	public abstract Status Execute();

	/// <summary>
	/// Reset node state (optional, override if needed)
	/// </summary>
	public virtual void Reset()
	{
		// Override in child classes if needed
	}
}
