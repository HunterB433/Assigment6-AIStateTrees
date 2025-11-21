using Godot;
using System;

// From Starter

/// <summary>
/// Action Leaf Node: Base class for action nodes
/// Actions perform actual behaviors (move, attack, etc.)
/// </summary>
public partial class BTAction : BTNode
{
	protected Func<Status> actionFunc;

	public BTAction(Func<Status> actionFunc)
	{
		this.actionFunc = actionFunc;
	}

	public override Status Execute()
	{
		if (actionFunc != null)
			return actionFunc();
		
		return Status.Failure;
	}
}
