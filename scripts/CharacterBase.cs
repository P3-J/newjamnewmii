using System;
using Godot;

public partial class CharacterBase : CharacterBody2D
{
    [Export]
    public int MaxHp = 10;

    [Export]
    public float Speed = 200.0f;

    [Export]
    AudioStreamPlayer hitsound;

    [Export]
    AnimatedSprite2D baseSprite;
    public int CurrentHp;
    public float charBaseProjSize;
    public int charBaseDmg;

    public float charBaseSize;

    public float charBaseProjSpeed;

    public float charBaseFireRate;

    public float charBaseSpreadMulti;

    public float wallPenChance;
    public ProgressBar hpbar;
    public Globals globals;
    public SignalBus sgbus;
    public bool canMove = true;
    private Timer hitTimer;

    public override void _Ready()
    {
        globals = GetNode<Globals>("/root/Globals");
        sgbus = GetNode<SignalBus>("/root/SignalBus");

        CurrentHp = MaxHp; // holyC
        UpdateStats();
        createHitFlash();
        // Traitply on new trait signal
        sgbus.Connect("NewTraitSelected", new Callable(this, nameof(UpdateStats)));
    }

    public void UpdateStats()
    {
        charBaseProjSize = globals.globalProjSizeMulti;

        charBaseDmg = globals.globalDamageMulti;

        charBaseSize = globals.globalCharSizeMulti;
        Scale = Vector2.One * charBaseSize;

        charBaseProjSpeed = globals.globalProjSpeedMulti;

        charBaseFireRate = globals.globalFireRate;

        charBaseSpreadMulti = globals.globalSpreadMulti;

        wallPenChance = globals.globalWallPenetrationChance;

        MaxHp += globals.extraHealth;
    }

    public virtual void TakeDmg(int dmg)
    {
        CurrentHp -= dmg;
        if (hitsound != null)
        {
            hitsound.Play();
        }

        HitFlash();

        if (CurrentHp <= 0)
        {
            Die();
        }
    }

    public void HitFlash()
    {
        if (baseSprite == null)
        {
            return;
        }
        baseSprite.Material.Set("shader_parameter/active", true);
        hitTimer.Start();
    }

    private void createHitFlash()
    {
        hitTimer = new Timer();
        hitTimer.WaitTime = 0.2f;
        hitTimer.OneShot = true;
        hitTimer.Timeout += TurnHitFlashOff;
        AddChild(hitTimer);
    }

    private void TurnHitFlashOff(){
       baseSprite.Material.Set("shader_parameter/active", false); 
    }

    public virtual void Die()
    {
        if (this.IsInGroup("Enemy"))
        {
            sgbus.EmitSignal("EnemyHasDied"); // emit this to check if room has cleared, room catches this.
            QueueFree();
        }
    }
}
