using Godot;

// From Class

/// <summary>
/// Sequence Node: Executes children in order until one fails
/// Returns Success only if ALL children succeed
/// Use case: Multi-step actions that must all complete
/// Example: "Check ammo AND aim AND shoot"
/// </summary>
public partial class BTSequence : BTComposite
{
	private int currentChildIndex = 0;

	public override Status Execute()
	{
		// If no children, fail
		if (children.Count == 0)
			return Status.Failure;
			
		currentChildIndex = 0;

		// Execute children in order
		while (currentChildIndex < children.Count)
		{
			Status childStatus = children[currentChildIndex].Execute();

			switch (childStatus)
			{
				case Status.Running:
					// Child still running, keep executing it next frame
					return Status.Running;
					
				case Status.Failure:
					// Child failed, sequence fails
					Reset();
					return Status.Failure;
					
				case Status.Success:
					// Child succeeded, move to next child
					currentChildIndex++;
					break;
			}
		}

		// All children succeeded
		Reset();
		return Status.Success;
	}

	public override void Reset()
	{
		base.Reset();
		currentChildIndex = 0;
	}
}
