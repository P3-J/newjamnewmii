using System;
using Godot;

public partial class CharacterBase : CharacterBody2D
{
    [Export]
    public float MaxHp = 10;

    [Export]
    public float Speed = 200.0f;

    public float charBaseProjSize;
    public float charBaseDmg;

    public float CurrentHp;
    public ProgressBar hpbar;
    public Globals globals;
    public SignalBus sgbus;

    public override void _Ready()
    {
        globals = GetNode<Globals>("/root/Globals");
        sgbus = GetNode<SignalBus>("/root/SignalBus");

        charBaseProjSize = globals.globalProjMulti;
        charBaseDmg = globals.globalDamageMulti;
        Scale = new(globals.globalCharSizeMulti, globals.globalCharSizeMulti);

        sgbus.Connect("NewTraitSelected", new Callable(this, nameof(UpdateStats)));
    }

    public void UpdateStats()
    {
        charBaseProjSize = globals.globalProjMulti;
        charBaseDmg = globals.globalDamageMulti;
    }

    public void TakeDmg(float dmg)
    {
        CurrentHp -= dmg;
        hpbar.Value = CurrentHp;

        if (CurrentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (this.IsInGroup("Enemy"))
        {
            sgbus.EmitSignal("EnemyHasDied"); // emit this to check if room has cleared, room catches this.
            QueueFree();
        }
    }
}
