using Godot;

public partial class Player : CharacterBody2D
{
	// ==========================================================
	// EXPORTED VARIABLES
	// ==========================================================
	[Export] public float Speed = 200f;
	[Export] public float AttackRange = 300f;
	[Export] public int MaxHealth = 100;

	// ==========================================================
	// INTERNAL STATE
	// ==========================================================
	public int CurrentHealth;

	// ==========================================================
	// READY
	// ==========================================================
	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
	}

	// ==========================================================
	// MOVEMENT + ATTACK
	// ==========================================================
	public override void _PhysicsProcess(double delta)
	{
		// Movement
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = inputDir * Speed;
		MoveAndSlide();

		// Player attack on SPACE (hits ALL enemies in range)
		if (Input.IsActionJustPressed("attack"))
		{
			var enemies = GetTree().GetNodesInGroup("Enemies");

			foreach (Node node in enemies)
			{
				if (node is Enemy enemy)
				{
					float dist = GlobalPosition.DistanceTo(enemy.GlobalPosition);

					if (dist <= AttackRange)
					{
						enemy.TakeDamage(10);
						GD.Print($"Player hit enemy at {dist}");
					}
				}
			}
		}

		QueueRedraw(); // redraw attack circle + health bar
	}

	// ==========================================================
	// HEALTH FUNCTIONS
	// ==========================================================
	public void TakeDamage(int amount)
	{
		CurrentHealth -= amount;

		if (CurrentHealth < 0)
			CurrentHealth = 0;

		QueueRedraw();
	}

	// ==========================================================
	// DRAW ATTACK RANGE + HEALTHBAR
	// ==========================================================
	public override void _Draw()
	{
		// PLAYER ATTACK RANGE CIRCLE
		DrawArc(
			Vector2.Zero,
			AttackRange,
			0,
			Mathf.Tau,
			48,
			new Color(0, 0, 1, 0.3f),
			2
		);

		// HEALTH BAR
		float barWidth = 80f;
		float barHeight = 10f;
		float yOffset = -100f;

		float healthPercent = (float)CurrentHealth / MaxHealth;

		// Background
		DrawRect(
			new Rect2(new Vector2(-barWidth / 2, yOffset), new Vector2(barWidth, barHeight)),
			new Color(0, 0, 0, 0.65f)
		);

		// Fill
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
