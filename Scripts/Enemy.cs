using Godot;

public partial class Enemy : CharacterBody2D
{
	[Export] public float MoveSpeed = 75f;
	[Export] public int PatrolPointCount = 8;
	[Export] public float Radius = 200f;
	[Export] public float ChaseRange = 400f;
	[Export] public float StopDistance = 200f; 
	[Export] public float AttackInterval = 1.0f;
	[Export] public int Damage = 10;
	[Export] public int MaxHealth = 100;
	[Export] public float HelpCooldown = 5f;
	[Export] public PackedScene AllyScene;
	
	// Audios
	[Export] public AudioStreamPlayer2D SfxPlayer;
	[Export] public AudioStream PatrolSfx;
	[Export] public AudioStream ChaseSfx;
	[Export] public AudioStream AttackSfx;
	[Export] public AudioStream FleeSfx;
	[Export] public AudioStream SummonSfx;

	public Player player;
	public int CurrentHealth;
	public double helpCooldownTimer = 0f;
	public int alliesSummoned = 0;
	public int maxAllies = 3;
	
	private Vector2[] patrolPoints;
	private BTNode behaviorTreeRoot;
	private Blackboard blackboard;
	private double attackTimer = 0f;
	private Player assistTarget = null;
	public void SetAssistTarget(Player p) => assistTarget = p;
	
	private string lastState = "";
	private Label stateLabel;

	public override void _Ready()
	{
		 stateLabel = GetNode<Label>("StateLabel");
		CurrentHealth = MaxHealth;
		player = GetTree().Root.GetNodeOrNull<Player>("Main/Player");
		CreatePatrolPoints();
		blackboard = new Blackboard();
		BuildBehaviorTree();
	}

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
	
	private void BuildBehaviorTree()
	{
		var root = new BTSelector();
		
		// flee
		var fleeSeq = new BTSequence();
		fleeSeq.AddChild(new BTIsHealthCritical(this));
		fleeSeq.AddChild(new BTFleeFromPlayer(this, MoveSpeed * 1.25f)); 
		root.AddChild(fleeSeq);
		
				// Summopn
		var helpSeq = new BTSequence();
		helpSeq.AddChild(new BTIsHealthLow(this));
		helpSeq.AddChild(new BTAreAlliesAvailable(this));
		helpSeq.AddChild(new BTCallHelpCooldownReady(this));
		helpSeq.AddChild(new BTSummonAlly(this));
		root.AddChild(helpSeq);
		
		// Attack
		var attackSeq = new BTSequence();
		attackSeq.AddChild(new BTIsPlayerInAttackRange(this, StopDistance));
		attackSeq.AddChild(new BTIsAttackCooldownReady(this));
		attackSeq.AddChild(new BTAttackPlayer(this));
		root.AddChild(attackSeq);
		
		// Chase
		var chaseSeq = new BTSequence();
		chaseSeq.AddChild(new BTCanSeePlayer(this, ChaseRange));
		chaseSeq.AddChild(new BTChasePlayer(this, MoveSpeed, StopDistance, ChaseRange));
		root.AddChild(chaseSeq);
		
		// Patroll
		root.AddChild(new BTPatrol(patrolPoints, MoveSpeed));
		behaviorTreeRoot = root;
		behaviorTreeRoot.Initialize(this, blackboard);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		helpCooldownTimer -= delta;
		attackTimer -= delta;  
		behaviorTreeRoot.Execute();
		QueueRedraw();
	}

	public override void _Draw()
	{
		if (patrolPoints != null)
		{
			foreach (var point in patrolPoints)
				DrawCircle(point - GlobalPosition, 6, Colors.Green);
			for (int i = 0; i < patrolPoints.Length; i++)
			{
				Vector2 a = patrolPoints[i] - GlobalPosition;
				Vector2 b = patrolPoints[(i + 1) % patrolPoints.Length] - GlobalPosition;
				DrawLine(a, b, Colors.Green, 2);
			}
		}
		DrawArc(
			Vector2.Zero,
			ChaseRange,
			0,
			Mathf.Tau,
			48,
			Colors.Yellow,
			2
		);
		DrawArc(
			Vector2.Zero,
			StopDistance,
			0,
			Mathf.Tau,
			48,
			Colors.Red, 
			2
		);
		
		float barWidth = 80f;
		float barHeight = 10f;
		float yOffset = -120f; 
		float hpPercent = (float)CurrentHealth / MaxHealth;
		DrawRect(
			new Rect2(new Vector2(-barWidth / 2, yOffset), new Vector2(barWidth, barHeight)),
			Colors.Black
		);
		Color hpColor = hpPercent > 0.5f ? Colors.Green : Colors.Red;
		DrawRect(
			new Rect2(
				new Vector2(-barWidth / 2, yOffset),
				new Vector2(barWidth * hpPercent, barHeight)
			),
			hpColor
		);
	}

	public bool IsAttackReady()
	{
		return attackTimer <= 0f;
	}

		public void DoAttack()
	{
		if (attackTimer <= 0f)
		{
			if (player != null)
			player.TakeDamage(Damage);
			attackTimer = AttackInterval;
		}
	}
	
		public void TakeDamage(int amount)
	{
		CurrentHealth -= amount;
		
		if (CurrentHealth < 0)
			QueueFree();
		
		QueueRedraw();
	}
	
public void PlayStateSfx(string state)
{
	if (state == lastState)
		return;
	lastState = state;
	if (stateLabel != null)
	{
		stateLabel.Text = state;
	}
	AudioStream sfx = null;
	switch (state)
	{
		case "Patrol":  sfx = PatrolSfx;  break;
		case "Chase":   sfx = ChaseSfx;   break;
		case "Attack":  sfx = AttackSfx;  break;
		case "Flee":    sfx = FleeSfx;    break;
		case "Summon":  sfx = SummonSfx;  break;
	}

	if (SfxPlayer != null && sfx != null)
	{
		SfxPlayer.Stream = sfx;
		SfxPlayer.Play();
	}
}


	
}
