using System;
using Godot;

public partial class CharacterBase : CharacterBody2D
{
    [Export] public int MaxHp = 10;
    [Export] public float Speed = 200.0f;
    [Export] AudioStreamPlayer hitsound;
    public int CurrentHp;
    public Globals globals;
    public SignalBus sgbus;

    public bool canMove = true;

    public override void _Ready()
    {
        globals = GetNode<Globals>("/root/Globals");
        sgbus = GetNode<SignalBus>("/root/SignalBus");


        CurrentHp = MaxHp; // holyC

    }

    public virtual void TakeDmg(int dmg)
    {
        CurrentHp -= dmg;
        if (hitsound != null){
            hitsound.Play();
        }

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
