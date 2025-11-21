using Godot;

public partial class BTAreAlliesAvailable : BTCondition
{
	private Enemy self;

	public BTAreAlliesAvailable(Enemy self)
		: base(null)
	{
		this.self = self;
		conditionFunc = Check;
	}

	private bool Check()
	{
		var allies = self.GetTree().GetNodesInGroup("Enemies");

		foreach (Node n in allies)
		{
			if (n != self)      // any enemy that is NOT this one
				return true;
		}

		return false;
	}
}
