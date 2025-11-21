using Godot;

public partial class BTChasePlayer : BTAction
{
	private CharacterBody2D agent;
	private float speed;
	private float stopDistance;
	private float chaseRange; 
	private Enemy enemy;
	
	public BTChasePlayer(CharacterBody2D agent, float speed, float stopDistance, float chaseRange)
		: base(null)
		{
			this.agent = agent;
			this.speed = speed;
			this.stopDistance = stopDistance;
			this.chaseRange = chaseRange;
			actionFunc = Chase;
		}
		

	public override void Initialize(Node2D agent, Blackboard blackboard)
	{
		base.Initialize(agent, blackboard);
		enemy = agent as Enemy; 
	}

	private Status Chase()
	{
		
		Node2D player = agent.GetTree().Root.GetNodeOrNull<Node2D>("Main/Player");
		if (player == null)
			return Status.Failure;
		float dist = agent.GlobalPosition.DistanceTo(player.GlobalPosition);
		if (dist > chaseRange)
		{
			agent.Velocity = Vector2.Zero;
			agent.MoveAndSlide();
			return Status.Failure;  
		}
		enemy?.PlayStateSfx("Chase");
		if (dist <= stopDistance)
		{
			agent.Velocity = Vector2.Zero;
			agent.MoveAndSlide();
			return Status.Success;  
		}
		Vector2 dir = (player.GlobalPosition - agent.GlobalPosition).Normalized();
		agent.Velocity = dir * speed;
		agent.MoveAndSlide();
		return Status.Running;
	}
}
