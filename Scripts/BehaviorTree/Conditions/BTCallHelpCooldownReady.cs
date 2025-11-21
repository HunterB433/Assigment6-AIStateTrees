using Godot;

public partial class BTCallHelpCooldownReady : BTCondition
{
	private Enemy enemy;
	public BTCallHelpCooldownReady(Enemy enemy)
		: base(null)
	{
		this.enemy = enemy;
		conditionFunc = Check;
	}
	private bool Check()
	{
		return enemy.helpCooldownTimer <= 0f;
	}
}
