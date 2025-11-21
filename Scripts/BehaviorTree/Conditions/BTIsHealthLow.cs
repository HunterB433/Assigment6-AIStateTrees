using Godot;

public partial class BTIsHealthLow : BTCondition
{
	private Enemy enemy;
	public BTIsHealthLow(Enemy enemy)
		: base(null)
	{
		this.enemy = enemy;
		conditionFunc = Check;
	}

	private bool Check()
	{
		float percent = (float)enemy.CurrentHealth / enemy.MaxHealth;
		return percent <= 0.5f;
	}
}
