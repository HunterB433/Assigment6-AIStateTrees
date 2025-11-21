using Godot;

public partial class BTCanSeePlayer : BTCondition
{
	private Node2D agent;
	private float chaseRange;
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
		float dist = agent.GlobalPosition.DistanceTo(player.GlobalPosition);
		bool inRange = dist <= chaseRange;
		return inRange;
	}
}
