using Godot;

public partial class BTFleeFromPlayer : BTAction
{
	private Enemy enemy;
	private float speed;
	public BTFleeFromPlayer(Enemy enemy, float speed)
		: base(null)
		{
			this.enemy = enemy;
			this.speed = speed;
			actionFunc = Flee;
		}

	private Status Flee()
	{
		if (enemy.player == null)
			return Status.Failure;
		Vector2 dir = (enemy.GlobalPosition - enemy.player.GlobalPosition).Normalized();
		enemy.Velocity = dir * speed;
		enemy.MoveAndSlide();
		return Status.Running;
	}
}
