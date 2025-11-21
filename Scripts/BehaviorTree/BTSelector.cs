using Godot;

/// <summary>
/// Selector Node: Executes children until one succeeds
/// Returns Failure only if ALL children fail
/// Use case: Priority-based decision making
/// Example: "Attack OR chase OR patrol"
/// </summary>
public partial class BTSelector : BTComposite
{
	private int currentChildIndex = 0;

	public override Status Execute()
	{
		// If no children, fail
		if (children.Count == 0)
			return Status.Failure;

		// Execute children in order until one succeeds
		while (currentChildIndex < children.Count)
		{
			Status childStatus = children[currentChildIndex].Execute();

			switch (childStatus)
			{
				case Status.Running:
					// Child still running, keep executing it
					return Status.Running;
					
				case Status.Success:
					// Child succeeded, selector succeeds
					Reset();
					return Status.Success;
					
				case Status.Failure:
					// Child failed, try next child
					currentChildIndex++;
					break;
			}
		}

		// All children failed
		Reset();
		return Status.Failure;
	}

	public override void Reset()
	{
		base.Reset();
		currentChildIndex = 0;
	}
}
