using System;
using System.Collections;
using Godot;

public partial class Enemy : CharacterBase
{
    [Export]
    public EnemyType enemytypeselection { get; set; }

    [Export]
    public Marker2D PatrolPathStart { get; set; }

    [Export]
    public PackedScene BulletScene;

    [Export]
    public AnimationPlayer WalkAnimPlayer;
    Marker2D PatrolPathEnd;
    NavigationAgent2D NavAgent;
    AnimatedSprite2D enemySprite;
    AudioStreamPlayer explosionsound;

    RayCast2D aggroray;
    Node2D rayparent;

    Timer rescantimer;
    Timer pathfindtimer;

    Vector2 globalShootPos;
    Area2D blowuparea;
    Sprite2D explosionSprite;

    public enum EnemyType
    {
        c4,
        makaron,
    }

    [Export]
    int AggroRange = 1000; // controls the raycast range for aggro

    private bool HeadingForEndPos = true;

    public enum EnemyState
    {
        patrol,
        aggro,
        afk,
    }

    public EnemyState CurrentState { get; set; }

    public ProgressBar hpbar;

    public override void _Ready()
    {
        NavAgent = GetNode<NavigationAgent2D>("navagent");
        base._Ready();

        hpbar = GetNode<ProgressBar>("hp");
        aggroray = GetNode<RayCast2D>("rayparent/aggroray");
        rayparent = GetNode<Node2D>("rayparent");
        enemySprite = GetNode<AnimatedSprite2D>("enemysprite");
        pathfindtimer = GetNode<Timer>("timers/pathfind");
        rescantimer = GetNode<Timer>("timers/rescan");
        blowuparea = GetNode<Area2D>("blowuparea");
        explosionSprite = GetNode<Sprite2D>("blowuparea/explosion");
        explosionsound = GetNode<AudioStreamPlayer>("blowuparea/explosionsound");

        hpbar.MaxValue = MaxHp;
        hpbar.Value = MaxHp;
        CurrentHp = MaxHp;

        aggroray.TargetPosition = new(AggroRange, 0);

        SetEnemyTypeSprite();
        SetScannersBasedOnType();

        if (PatrolPathStart != null)
        {
            PatrolPathEnd = PatrolPathStart.GetChild<Marker2D>(0); // super bad practice, just ez for this
            CurrentState = EnemyState.patrol;
            NavAgent.TargetPosition = PatrolPathStart.GlobalPosition; // initial go to patrol path
        }
        else
        {
            CurrentState = EnemyState.afk;
        }
    }

    private void SetScannersBasedOnType()
    {
        if (enemytypeselection == EnemyType.makaron)
        {
            blowuparea.Visible = false;
        }
    }

    public override void _Process(double delta)
    {
        RayLookAtPlayerAndProcAggro();

        if (!canMove)
            return;

        if (CurrentState == EnemyState.afk)
        {
            WalkAnimPlayer.Play("RESET");
        }

        if (!NavAgent.IsNavigationFinished())
        {
            WalkAnimPlayer.Play("wlak");
            Vector2 nextPoint = NavAgent.GetNextPathPosition();
            Vector2 direction = (nextPoint - GlobalPosition).Normalized();
            Velocity = direction * Speed;
            enemySprite.FlipH = direction.X > 0;
            MoveAndSlide();
        }
        else
        {
            WalkAnimPlayer.Play("RESET");
            Velocity = Vector2.Zero;
            HeadingForEndPos = !HeadingForEndPos;
            if (CurrentState == EnemyState.patrol)
            {
                if (HeadingForEndPos)
                {
                    NavAgent.TargetPosition = PatrolPathEnd.GlobalPosition;
                }
                else
                {
                    NavAgent.TargetPosition = PatrolPathStart.GlobalPosition;
                }
            }
        }
    }

    private void _on_rescan_timeout()
    {
        shootProjectileAt(GetPlayerPos());
        rescantimer.WaitTime = globals.globalFireRate / 800f;
        canMove = true;
    }

    public override void TakeDmg(int dmg)
    {
        base.TakeDmg(dmg);
        hpbar.Value = CurrentHp;
        GD.Print("Enemy took: ", dmg, " dmg");
    }

    public void _on_blowuparea_body_entered(Node2D area)
    {
        if (area.IsInGroup("Player"))
        {
            // could put a timer here so they stop and then blow but ok
            // blow up
            explosionsound.Play();
            explosionSprite.Visible = true;
            globals.player.TakeDmg(globals.extraDamage + charBaseDmg);
            canMove = false;
        }
    }

    private void shootProjectileAt(Vector2 pos)
    {
        Bullet bullet = (Bullet)BulletScene.Instantiate();
        bullet.Position = GlobalPosition;
        bullet.BulletOwner = this;
        bullet.dmg = charBaseDmg;

        Vector2 Direction = (pos - GlobalPosition).Normalized();

        Random random = new();
        // 0 - no spread
        // -1 or 1 - spread direction
        int SpreadDirectionMultiplier = random.Next(-1, 2);
        float SpreadValue = (float)(
            random.NextDouble() * charBaseSpreadMulti * SpreadDirectionMultiplier
        );

        Vector2 spreadDirection = Direction.Rotated(SpreadValue);
        bullet.Direction = spreadDirection;

        bullet.dmg = charBaseDmg + globals.extraDamage;
        bullet.Scale = Vector2.One * (charBaseProjSize * globals.globalPlayerStatMulti);
        bullet.wallPenChance = wallPenChance;
        bullet.Speed *= charBaseProjSpeed * globals.globalPlayerStatMulti;

        bullet.LookAt(bullet.Position + spreadDirection);
        GetTree().CurrentScene.AddChild(bullet);
    }

    private void _on_pathfind_timeout()
    {
        if (CurrentState == EnemyState.aggro)
        {
            NavAgent.TargetPosition = GetPlayerPos();
        }
        if (enemytypeselection == EnemyType.c4 && !canMove)
        {
            Die();
        }
    }

    private void SetEnemyTypeSprite()
    {
        switch (enemytypeselection)
        {
            case EnemyType.c4:
                enemySprite.Animation = "tseneli";
                break;
            case EnemyType.makaron:
                enemySprite.Animation = "makaron";
                break;
        }
    }

    private void RayLookAtPlayerAndProcAggro()
    {
        rayparent.LookAt(globals.player.GlobalPosition);
        Node collider = (Node)aggroray.GetCollider();

        if (collider == null)
        {
            return;
        }
        if (collider.IsInGroup("Player"))
        {
            CurrentState = EnemyState.aggro;
            enemySprite.FlipH = GlobalPosition.X < GetPlayerPos().X;
            MobActionBasedOnType();
        }
    }

    private void MobActionBasedOnType()
    {
        if (enemytypeselection == EnemyType.c4)
        {
            NavAgent.TargetPosition = GetPlayerPos();
            return;
        }

        if (enemytypeselection == EnemyType.makaron)
        {
            canMove = false;
            if (rescantimer.IsStopped())
            {
                globalShootPos = GetPlayerPos(); // refresh shoot pos coolio
                rescantimer.Start();
            }
        }
    }

    private Vector2 GetPlayerPos()
    {
        return globals.player.GlobalPosition;
    }
}
