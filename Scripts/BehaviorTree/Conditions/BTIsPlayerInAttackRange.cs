using Godot;

public partial class BTIsPlayerInAttackRange : BTCondition
{
	private Node2D agent;
	private float attackRange;

	public BTIsPlayerInAttackRange(Node2D agent, float attackRange)
		: base(null)
	{
		this.agent = agent;
		this.attackRange = attackRange;

		conditionFunc = Check;
	}

	private bool Check()
	{
		Node2D player = agent.GetTree().Root.GetNodeOrNull<Node2D>("Main/Player");
		if (player == null)
			return false;

		float dist = agent.GlobalPosition.DistanceTo(player.GlobalPosition);
		return dist <= attackRange;
	}
}
