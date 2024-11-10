using System;
using Godot;

public partial class CharacterBase : CharacterBody2D
{
    [Export]
    public int MaxHp = 10;

    [Export]
    public float Speed = 200.0f;
    public int CurrentHp;
    public ProgressBar hpbar;
    public Globals globals;
    public SignalBus sgbus;

    public override void _Ready()
    {
        globals = GetNode<Globals>("/root/Globals");
        sgbus = GetNode<SignalBus>("/root/SignalBus");

    }

    public void TakeDmg(int dmg)
    {
        CurrentHp -= dmg;
        hpbar.Value = CurrentHp;

        if (CurrentHp <= 0)
        {
            Die();
        }
    }

    public void Die(){
        if (this.IsInGroup("Enemy")){
            sgbus.EmitSignal("EnemyHasDied"); // emit this to check if room has cleared, room catches this.
            QueueFree();
        }

    }

}
