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

    public float charBaseSize;

    public float charBaseProjSpeed;

    public float charBaseFireRate;

    public float charBaseSpreadMulti;

    public float wallPenChance;

    public float CurrentHp;
    public ProgressBar hpbar;
    public Globals globals;
    public SignalBus sgbus;

    public override void _Ready()
    {
        globals = GetNode<Globals>("/root/Globals");
        sgbus = GetNode<SignalBus>("/root/SignalBus");

        // Trait apply on init
        UpdateStats();

        // Trait apply on new trait signal
        sgbus.Connect("NewTraitSelected", new Callable(this, nameof(UpdateStats)));
    }

    public void UpdateStats()
    {
        charBaseProjSize = globals.globalProjSizeMulti;

        charBaseDmg = globals.globalDamageMulti;

        charBaseSize = globals.globalCharSizeMulti;
        Scale = Vector2.One * charBaseSize;

        charBaseProjSpeed = globals.globalProjSpeedMulti;

        charBaseFireRate = globals.globalFireRateMulti;

        charBaseSpreadMulti = globals.globalSpreadMulti;

        wallPenChance = globals.globalWallPenetrationChance;

        MaxHp *= globals.globalHealthMulti;
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
