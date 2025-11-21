using Godot;

public partial class BTAreAlliesAvailable : BTCondition
{
	private Enemy enemy;

	public BTAreAlliesAvailable(Enemy enemy)
		: base(null)
	{
		this.enemy = enemy;
		conditionFunc = Check;
	}

	private bool Check()
	{
		return (enemy.alliesSummoned < enemy.maxAllies );
	}
}
