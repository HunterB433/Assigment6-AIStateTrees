using Godot;

public partial class BTIsAttackCooldownReady : BTCondition
{
	private Enemy enemy;

	public BTIsAttackCooldownReady(Enemy enemy)
		: base(null)
	{
		this.enemy = enemy;
		conditionFunc = Check;
	}

	private bool Check()
	{
		return enemy.IsAttackReady();
	}
}
