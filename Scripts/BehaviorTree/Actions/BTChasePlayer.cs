using Godot;

public partial class BTChasePlayer : BTAction
{
	private CharacterBody2D agent;
	private float speed;
	private float stopDistance;

	public BTChasePlayer(CharacterBody2D agent, float speed, float stopDistance)
		: base(null)
	{
		this.agent = agent;
		this.speed = speed;
		this.stopDistance = stopDistance;

		actionFunc = Chase;
	}

	private Status Chase()
	{
		Node2D player = agent.GetTree().Root.GetNodeOrNull<Node2D>("Main/Player");
		if (player == null)
			return Status.Failure;

		float dist = agent.GlobalPosition.DistanceTo(player.GlobalPosition);

		// STOP CHASING when within attack distance
		if (dist <= stopDistance)
		{
			agent.Velocity = Vector2.Zero;
			agent.MoveAndSlide();
			return Status.Success;  // allows AttackSequence to run
		}

		// MOVE TOWARD PLAYER
		Vector2 dir = (player.GlobalPosition - agent.GlobalPosition).Normalized();
		agent.Velocity = dir * speed;
		agent.MoveAndSlide();

		return Status.Running;
	}
}
