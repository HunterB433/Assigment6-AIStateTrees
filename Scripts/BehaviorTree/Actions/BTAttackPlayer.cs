using Godot;

public partial class BTAttackPlayer : BTAction
{
	private Enemy enemy;
	
	public BTAttackPlayer(Enemy enemy)
		: base(null)
	{
		this.enemy = enemy;
		actionFunc = Attack;
	}
	private Status Attack()
	{
		enemy.PlayStateSfx("Attack");
		enemy.DoAttack();
		return Status.Running;  
	}
}
