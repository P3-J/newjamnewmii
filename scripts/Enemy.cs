using System;
using Godot;

public partial class Enemy : CharacterBase
{
    [Export] public EnemyType enemytypeselection {get; set;}
    [Export] public Marker2D PatrolPathStart {get; set;}
    
    Marker2D PatrolPathEnd;
    NavigationAgent2D NavAgent;
    AnimatedSprite2D enemySprite;
    
    RayCast2D aggroray;
    Node2D rayparent;

  
    public enum EnemyType{
        c4,
        makaron
    }

    public int AggroRange; // controls the raycast range for aggro

    private bool HeadingForEndPos = true;

    public enum EnemyState { patrol, aggro, afk }
    public EnemyState CurrentState {get; set;}

    public override void _Ready()
    {
        NavAgent = GetNode<NavigationAgent2D>("navagent");
        base._Ready();

        hpbar = GetNode<ProgressBar>("hp");
        aggroray = GetNode<RayCast2D>("rayparent/aggroray");
        rayparent = GetNode<Node2D>("rayparent");
        enemySprite = GetNode<AnimatedSprite2D>("enemysprite");
        
        hpbar.MaxValue = MaxHp;
        hpbar.Value = MaxHp;
        CurrentHp = MaxHp;

        SetEnemyTypeSprite();

        if (PatrolPathStart != null){
            PatrolPathEnd = PatrolPathStart.GetChild<Marker2D>(0); // super bad practice, just ez for this
            CurrentState = EnemyState.patrol;
            NavAgent.TargetPosition = PatrolPathStart.GlobalPosition; // initial go to patrol path
        } else {
            CurrentState = EnemyState.aggro;
            NavAgent.TargetPosition = GetPlayerPos();
        }
    
    }

    public override void _Process(double delta)
    {
        RayLookAtPlayerAndProcAggro();

  
        if (CurrentState == EnemyState.aggro){
            NavAgent.TargetPosition = GetPlayerPos();
        }
        
        if (!NavAgent.IsNavigationFinished())
        {
            Vector2 nextPoint = NavAgent.GetNextPathPosition();
            Vector2 direction = (nextPoint - GlobalPosition).Normalized();
            Velocity = direction * Speed;
            enemySprite.FlipH = direction.X > 0;
            MoveAndSlide();

        }
        else
        {
            Velocity = Vector2.Zero;
            HeadingForEndPos = !HeadingForEndPos;
            if (CurrentState == EnemyState.patrol){
                if (HeadingForEndPos){
                    NavAgent.TargetPosition = PatrolPathEnd.GlobalPosition;
                } else {
                    NavAgent.TargetPosition = PatrolPathStart.GlobalPosition;
                }
            }
        }
    }

    private void SetEnemyTypeSprite(){
        switch (enemytypeselection){
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

        if (collider == null) {return;}
        if (collider.IsInGroup("Player")) {CurrentState = EnemyState.aggro;}

    }


    private Vector2 GetPlayerPos(){
        return globals.player.GlobalPosition;
    }


}
