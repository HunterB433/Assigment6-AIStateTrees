using Godot;

public partial class BTCanSeePlayer : BTCondition
{
	private Node2D agent;
	private float chaseRange;

	private ulong lastDebugTime = 0;

	public BTCanSeePlayer(Node2D agent, float chaseRange)
		: base(null)
	{
		this.agent = agent;
		this.chaseRange = chaseRange;

		conditionFunc = Check;
	}

	private bool Check()
	{
		Node2D player = agent.GetTree().Root.GetNodeOrNull<Node2D>("Main/Player");
		if (player == null)
		{
			DebugPrintOncePerSecond("Player NOT FOUND");
			return false;
		}

		float dist = agent.GlobalPosition.DistanceTo(player.GlobalPosition);
		bool inRange = dist <= chaseRange;

		DebugPrintOncePerSecond(
			$"BTCanSeePlayer → dist={dist:F2}, chaseRange={chaseRange}, inRange={inRange}"
		);

		return inRange;
	}

	private void DebugPrintOncePerSecond(string msg)
	{
		ulong now = Time.GetTicksMsec();

		if (now - lastDebugTime >= 1000)
		{
			GD.Print(msg);
			lastDebugTime = now;
		}
	}
}
