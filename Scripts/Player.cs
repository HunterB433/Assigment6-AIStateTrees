using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 200f;
	[Export] public float AttackRange = 300f;
	[Export] public int MaxHealth = 100;
	public int CurrentHealth;
	
	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = inputDir * Speed;
		MoveAndSlide();
		if (Input.IsActionJustPressed("attack"))
		{
			var enemies = GetTree().GetNodesInGroup("Enemies");
			foreach (Node node in enemies)
			{
			if (node is Enemy enemy)
				{
					float dist = GlobalPosition.DistanceTo(enemy.GlobalPosition);
					if (dist <= AttackRange)
						enemy.TakeDamage(10);
				}
				else if (node is Ally ally)
				{
					float dist = GlobalPosition.DistanceTo(ally.GlobalPosition);
					if (dist <= AttackRange)
						ally.TakeDamage(10);
				}
			}
		}
		QueueRedraw();
	}

	public void TakeDamage(int amount)
	{
		CurrentHealth -= amount;
		if (CurrentHealth <= 0)
			GetTree().ChangeSceneToFile("res://scenes/LossScreen.tscn");
		QueueRedraw();
	}

	public override void _Draw()
	{
		DrawArc(
			Vector2.Zero,
			AttackRange,
			0,
			Mathf.Tau,
			48,
			new Color(0, 0, 1, 0.3f),
			2
		);
		float barWidth = 80f;
		float barHeight = 10f;
		float yOffset = -100f;
		float healthPercent = (float)CurrentHealth / MaxHealth;
		DrawRect(
			new Rect2(new Vector2(-barWidth / 2, yOffset), new Vector2(barWidth, barHeight)),
			new Color(0, 0, 0, 0.65f)
		);
		Color hpColor = (healthPercent > 0.5f)
			? new Color(0, 1, 0)
			: new Color(1, 0.3f, 0);
		DrawRect(
			new Rect2(
				new Vector2(-barWidth / 2, yOffset),
				new Vector2(barWidth * healthPercent, barHeight)
			),
			hpColor
		);
	}
}
