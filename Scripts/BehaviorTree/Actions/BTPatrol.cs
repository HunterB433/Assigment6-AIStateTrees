using Godot;

public partial class BTPatrol : BTAction
{
	private int patrolIndex = 0;
	private Vector2[] points;
	private Enemy enemy;
	
	public BTPatrol(Vector2[] patrolPoints, float speed)
		: base(null)
	{
		points = patrolPoints;
		actionFunc = ExecutePatrol;
		this.speed = speed;
	}

	private float speed;
		public override void Initialize(Node2D agent, Blackboard blackboard)
	{
		base.Initialize(agent, blackboard);
		enemy = agent as Enemy; 
	}

	private Status ExecutePatrol()
	{
		enemy?.PlayStateSfx("Patrol");
		if (points == null || points.Length == 0)
			return Status.Failure;
		var pos = agent.GlobalPosition;
		var target = points[patrolIndex];
		float dist = pos.DistanceTo(target);
		if (dist < 10f)
		{
			patrolIndex = (patrolIndex + 1) % points.Length;
			return Status.Success;
		}
		Vector2 dir = (target - pos).Normalized();
		(agent as CharacterBody2D).Velocity = dir * speed;
		(agent as CharacterBody2D).MoveAndSlide();
		return Status.Running;
	}
}
