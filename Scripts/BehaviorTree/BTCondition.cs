using Godot;
using System;

// From Class

/// <summary>
/// Condition Leaf Node: Base class for condition checks
/// Conditions evaluate game state and return Success/Failure
/// Should NOT have side effects (read-only)
/// </summary>
public partial class BTCondition : BTNode
{
	protected Func<bool> conditionFunc;

	public BTCondition(Func<bool> conditionFunc)
	{
		this.conditionFunc = conditionFunc;
	}

	public override Status Execute()
	{
		if (conditionFunc != null)
			return conditionFunc() ? Status.Success : Status.Failure;
		
		return Status.Failure;
	}
}
