using Godot;

public partial class Ally : CharacterBody2D
{

	[Export] public float AttackRange = 300f;
	[Export] public float AttackInterval = 1.5f;
	[Export] public int Damage = 10;
	[Export] public int MaxHealth = 50;

	private Player player;
	private float attackTimer = 0f;
	private int currentHealth = 50;

	public override void _Ready()
	{
		AddToGroup("Enemies"); // Just in case
		currentHealth = MaxHealth;
		player = GetTree().Root.GetNodeOrNull<Player>("Main/Player");
	}

	public override void _PhysicsProcess(double delta)
	{
		attackTimer -= (float)delta;
		if (player != null)
		{
			float dist = GlobalPosition.DistanceTo(player.GlobalPosition);
			if (dist <= AttackRange && attackTimer <= 0f)
			{
				player.TakeDamage(Damage);
				attackTimer = AttackInterval;
			}
		}
		QueueRedraw();
	}


	public void TakeDamage(int amount)
	{
		currentHealth -= amount;
		if (currentHealth <= 0)
			QueueFree();
		QueueRedraw(); // for HP bar
		if (currentHealth <= 0)
		{
			QueueFree();
		}
	}

	public override void _Draw()
	{
		DrawArc(Vector2.Zero, AttackRange,0,Mathf.Tau,48, Colors.Purple, 2);

		// Health bar
		float barWidth = 60f;
		float barHeight = 8f;
		float yOffset = -80f;
		float hpPercent = (float)currentHealth / MaxHealth;
		DrawRect(
			new Rect2(new Vector2(-barWidth / 2, yOffset), new Vector2(barWidth, barHeight)),
			Colors.Black
		);
		Color hpColor = hpPercent > 0.5f
			? Colors.Green
			: Colors.Red;
				DrawRect(
			new Rect2(
				new Vector2(-barWidth / 2, yOffset),  
				new Vector2(barWidth * hpPercent, barHeight)
			),
			hpColor
		);
	}
}
