using Godot;

public partial class Enemy : CharacterBody2D
{
	// ==========================================================
	// EXPORTED VARIABLES
	// ==========================================================
	[Export] public float MoveSpeed = 75f;
	[Export] public int PatrolPointCount = 8;
	[Export] public float Radius = 200f;
	[Export] public float ChaseRange = 400f;
	[Export] public float StopDistance = 200f;   // attack distance
	[Export] public float AttackInterval = 1.0f;
	[Export] public int Damage = 10;
	[Export] public int MaxHealth = 100;
	[Export] public float HelpCooldown = 15f;
	
		public Player player;
		public int CurrentHealth;
		public double helpCooldownTimer = 15f;

	// ==========================================================
	// INTERNAL STATE
	// ==========================================================

	private Vector2[] patrolPoints;
	private BTNode behaviorTreeRoot;
	private Blackboard blackboard;

	private double attackTimer = 0f;
	
	private Player assistTarget = null;
	public void SetAssistTarget(Player p) => assistTarget = p;

	// ==========================================================
	// READY
	// ==========================================================
	public override void _Ready()
	{
		// Allies assisting: move toward player if assistTarget assigned
if (assistTarget != null)
{
	Vector2 dir = (assistTarget.GlobalPosition - GlobalPosition).Normalized();
	Velocity = dir * MoveSpeed;
	MoveAndSlide();
}
		CurrentHealth = MaxHealth;
		player = GetTree().Root.GetNodeOrNull<Player>("Main/Player");
		CreatePatrolPoints();
		blackboard = new Blackboard();
		BuildBehaviorTree();
	}

	// ==========================================================
	// PATROL POINT CREATION
	// ==========================================================
	private void CreatePatrolPoints()
	{
		patrolPoints = new Vector2[PatrolPointCount];

		for (int i = 0; i < PatrolPointCount; i++)
		{
			float angle = (Mathf.Tau / PatrolPointCount) * i;

			patrolPoints[i] = GlobalPosition + new Vector2(
				Mathf.Cos(angle) * Radius,
				Mathf.Sin(angle) * Radius
			);
		}
	}

	// ==========================================================
	// BEHAVIOR TREE SETUP
	// ==========================================================
	private void BuildBehaviorTree()
	{
		var root = new BTSelector();
		
		var helpSeq = new BTSequence();
		helpSeq.AddChild(new BTIsHealthLow(this));
		helpSeq.AddChild(new BTAreAlliesAvailable(this));
		helpSeq.AddChild(new BTCallHelpCooldownReady(this));
		helpSeq.AddChild(new BTSummonAlly(this));
		root.AddChild(helpSeq);
		// ------------------------------
		// ATTACK SEQUENCE
		// ------------------------------
		var attackSeq = new BTSequence();
		attackSeq.AddChild(new BTIsPlayerInAttackRange(this, StopDistance));
		attackSeq.AddChild(new BTIsAttackCooldownReady(this));
		attackSeq.AddChild(new BTAttackPlayer(this));
		root.AddChild(attackSeq);

		// ------------------------------
		// CHASE SEQUENCE
		// ------------------------------
		var chaseSeq = new BTSequence();
		chaseSeq.AddChild(new BTCanSeePlayer(this, ChaseRange));
		chaseSeq.AddChild(new BTChasePlayer(this, MoveSpeed, StopDistance));
		root.AddChild(chaseSeq);

		// ------------------------------
		// PATROL (fallback)
		// ------------------------------
		root.AddChild(new BTPatrol(patrolPoints, MoveSpeed));

		behaviorTreeRoot = root;
		behaviorTreeRoot.Initialize(this, blackboard);
	}

	// ==========================================================
	// PROCESS LOOP
	// ==========================================================
	public override void _PhysicsProcess(double delta)
	{
		helpCooldownTimer -= delta;
		attackTimer -= delta;   // tick cooldown every frame
		behaviorTreeRoot.Execute();
		QueueRedraw();
	}

	// ==========================================================
	// DEBUG DRAWING
	// ==========================================================
	public override void _Draw()
	{
		// Draw patrol points + path
		if (patrolPoints != null)
		{
			foreach (var point in patrolPoints)
				DrawCircle(point - GlobalPosition, 6, new Color(0, 1, 0, 0.6f));

			for (int i = 0; i < patrolPoints.Length; i++)
			{
				Vector2 a = patrolPoints[i] - GlobalPosition;
				Vector2 b = patrolPoints[(i + 1) % patrolPoints.Length] - GlobalPosition;
				DrawLine(a, b, new Color(0, 1, 0, 0.3f), 2);
			}
		}

		// Draw chase range
		DrawArc(
			Vector2.Zero,
			ChaseRange,
			0,
			Mathf.Tau,
			48,
			new Color(1, 0.85f, 0, 0.3f),
			2
		);

		// Draw attack (stop) distance
		DrawArc(
			Vector2.Zero,
			StopDistance,
			0,
			Mathf.Tau,
			48,
			new Color(1, 0, 0, 0.3f),  // red-ish
			2
		);
		
		// ==========================================
// ENEMY HEALTH BAR
// ==========================================
float barWidth = 80f;
float barHeight = 10f;
float yOffset = -120f; // higher than patrol circles

float hpPercent = (float)CurrentHealth / MaxHealth;

// background
DrawRect(
	new Rect2(new Vector2(-barWidth / 2, yOffset), new Vector2(barWidth, barHeight)),
	new Color(0, 0, 0, 0.65f)
);

// health fill
Color hpColor = hpPercent > 0.5f ? new Color(0, 1, 0) : new Color(1, 0.3f, 0);

DrawRect(
	new Rect2(
		new Vector2(-barWidth / 2, yOffset),
		new Vector2(barWidth * hpPercent, barHeight)
	),
	hpColor
);

	}

	// ==========================================================
	// ATTACK HELPERS
	// ==========================================================
	public bool IsAttackReady()
	{
		return attackTimer <= 0f;
	}

		public void DoAttack()
	{
		if (attackTimer <= 0f)
		{
			GD.Print("ATTACK FIRED!");
			
			if (player != null)
			player.TakeDamage(Damage);
			
			attackTimer = AttackInterval;
		}
	}
	
		public void TakeDamage(int amount)
	{
		CurrentHealth -= amount;
		
		if (CurrentHealth < 0)
			CurrentHealth = 0;
		
		GD.Print("Enemy HP = ", CurrentHealth);
		
		QueueRedraw();
	}
}
