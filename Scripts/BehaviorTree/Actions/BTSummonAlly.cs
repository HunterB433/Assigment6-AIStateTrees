using Godot;

public partial class BTSummonAlly : BTAction
{
	private Enemy caller;

	public BTSummonAlly(Enemy caller)
		: base(null)
	{
		this.caller = caller;

		actionFunc = CallHelp;
	}

	private Status CallHelp()
	{
		var allies = caller.GetTree().GetNodesInGroup("Enemies");

		foreach (Node n in allies)
		{
			if (n is Enemy ally && ally != caller)
			{
				ally.SetAssistTarget(caller.player);  
			}
		}

		caller.helpCooldownTimer = caller.HelpCooldown;

		GD.Print("Enemy called for help!");

		return Status.Success;
	}
}
