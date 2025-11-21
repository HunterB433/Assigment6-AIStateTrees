using Godot;

public partial class BTSummonAlly : BTAction
{
	private Enemy caller;

	public BTSummonAlly(Enemy caller)
		: base(null)
	{
		this.caller = caller;
		actionFunc = Summon;
	}

	private Status Summon()
	{
		if (caller.AllyScene == null)
		{
			return Status.Failure;
		}
		
		Node2D ally = caller.AllyScene.Instantiate<Node2D>();
		Node parent = caller.GetTree().CurrentScene;
		
		parent.AddChild(ally);
		ally.GlobalPosition = caller.GlobalPosition + new Vector2(80, 0);
		
		caller.PlayStateSfx("Summon");
		caller.helpCooldownTimer = caller.HelpCooldown;
		caller.alliesSummoned++;
		
		return Status.Success;
	}
}
