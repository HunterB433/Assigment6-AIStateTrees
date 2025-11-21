using Godot;

public partial class LossButton : Control
{
	public void _on_retry_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/Main.tscn");
	}
}
