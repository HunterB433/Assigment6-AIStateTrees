using Godot;

public partial class BTIsHealthCritical : BTCondition
{
	private Enemy enemy;
	private float threshold;
	public BTIsHealthCritical(Enemy enemy, float threshold = 0.2f)
		: base(null)
	{
		this.enemy = enemy;
		this.threshold = threshold;
		conditionFunc = Check;
	}

	private bool Check()
	{
		float percent = (float)enemy.CurrentHealth / enemy.MaxHealth;
		return percent <= threshold;
	}
}
