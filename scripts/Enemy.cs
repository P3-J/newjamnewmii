using System;
using Godot;

public partial class Enemy : CharacterBase
{

    NavigationAgent2D NavAgent;

    
    RayCast2D aggroray;
    Node2D rayparent;

    public override void _Ready()
    {
        NavAgent = GetNode<NavigationAgent2D>("navagent");
        base._Ready();
        //CheckForTraitsToApply();

        hpbar = GetNode<ProgressBar>("hp");
        aggroray = GetNode<RayCast2D>("rayparent/aggroray");
        rayparent = GetNode<Node2D>("rayparent");
        
        hpbar.MaxValue = MaxHp;
        hpbar.Value = MaxHp;
        CurrentHp = MaxHp;

        NavAgent.TargetPosition = GetPlayerPos();
    }

    public override void _Process(double delta)
    {

        NavAgent.TargetPosition = GetPlayerPos();

        RayLookAtPlayer();

        if (!NavAgent.IsNavigationFinished())
        {
            
            Vector2 nextPoint = NavAgent.GetNextPathPosition();
            Vector2 direction = (nextPoint - GlobalPosition).Normalized();
            Velocity = direction * Speed;
            MoveAndSlide();

        }
        else
        {
            Velocity = Vector2.Zero;
        }
    }

    private void CheckForTraitsToApply()
    {
        throw new NotImplementedException();
    }


    private void RayLookAtPlayer()
    {
        rayparent.LookAt(globals.player.GlobalPosition);
    }


    private Vector2 GetPlayerPos(){
        return globals.player.GlobalPosition;
    }


}
